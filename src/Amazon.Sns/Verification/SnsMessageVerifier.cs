using System.Collections.Concurrent;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;

namespace Amazon.Sns;

// Verifies the signature of the messages SNS delivers to HTTP(S) endpoints.
// https://docs.aws.amazon.com/sns/latest/dg/sns-verify-signature-of-message-verify-message-signature.html

public sealed partial class SnsMessageVerifier
{
    private static readonly string[] s_notificationFields = ["Message", "MessageId", "Subject", "Timestamp", "TopicArn", "Type"];
    private static readonly string[] s_confirmationFields = ["Message", "MessageId", "SubscribeURL", "Timestamp", "Token", "TopicArn", "Type"];

    private readonly HttpClient _httpClient;
    private readonly ConcurrentDictionary<string, X509Certificate2> _certificates = new();

    public SnsMessageVerifier()
        : this(new HttpClient(new SocketsHttpHandler { AllowAutoRedirect = false }) { Timeout = TimeSpan.FromSeconds(10) })
    {
    }

    // The client should not follow redirects
    public SnsMessageVerifier(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        _httpClient = httpClient;
    }

    /// <summary>
    /// Verifies the raw JSON body of a message. The signature does not cover MessageAttributes,
    /// and callers should also reject any TopicArn they do not expect.
    /// </summary>
    /// <returns>True if the message was signed by a certificate served by SNS</returns>
    /// <exception cref="HttpRequestException">The certificate could not be downloaded</exception>
    public async Task<bool> VerifyAsync(string json, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(json);

        // The signature is over the original text of each field (e.g. Timestamp), so this reads the JSON rather than an SnsMessage
        if (!TryReadFields(json, out var fields)
            || !fields.TryGetValue("Signature", out var signatureText)
            || !fields.TryGetValue("SigningCertURL", out var certificateUrl)
            || !fields.TryGetValue("SignatureVersion", out var signatureVersion)
            || !fields.TryGetValue("Type", out var type))
        {
            return false;
        }

        HashAlgorithmName? hashAlgorithm = signatureVersion switch {
            "1" => HashAlgorithmName.SHA1,
            "2" => HashAlgorithmName.SHA256,
            _   => null
        };

        string[]? signedFields = type switch {
            "Notification"                                       => s_notificationFields,
            "SubscriptionConfirmation" or "UnsubscribeConfirmation" => s_confirmationFields,
            _                                                    => null
        };

        if (hashAlgorithm is null || signedFields is null || !IsTrustedCertificateUrl(certificateUrl))
        {
            return false;
        }

        var signature = new byte[(signatureText.Length / 4 * 3) + 3];

        if (!Convert.TryFromBase64String(signatureText, signature, out int signatureLength)
            || !TryGetStringToSign(fields, signedFields, out var stringToSign))
        {
            return false;
        }

        var certificate = await GetCertificateAsync(certificateUrl, cancellationToken).ConfigureAwait(false);

        var now = DateTime.Now;

        if (certificate is null || now < certificate.NotBefore || now > certificate.NotAfter)
        {
            return false;
        }

        using var publicKey = certificate.GetRSAPublicKey();

        return publicKey is not null && publicKey.VerifyData(
            Encoding.UTF8.GetBytes(stringToSign),
            signature.AsSpan(0, signatureLength),
            hashAlgorithm.Value,
            RSASignaturePadding.Pkcs1);
    }

    internal static bool IsTrustedCertificateUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && uri.Scheme == Uri.UriSchemeHttps
            && uri.IsDefaultPort
            && uri.UserInfo.Length is 0
            && uri.Query.Length is 0
            && uri.AbsolutePath.EndsWith(".pem", StringComparison.Ordinal)
            && SnsHostRegex().IsMatch(uri.Host);
    }

    private async Task<X509Certificate2?> GetCertificateAsync(string url, CancellationToken cancellationToken)
    {
        if (_certificates.TryGetValue(url, out var certificate))
        {
            return certificate;
        }

        using var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode || response.RequestMessage?.RequestUri is Uri finalUri && !IsTrustedCertificateUrl(finalUri.AbsoluteUri))
        {
            return null;
        }

        string pem = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            return _certificates.GetOrAdd(url, X509Certificate2.CreateFromPem(pem));
        }
        catch (Exception ex) when (ex is CryptographicException or ArgumentException)
        {
            return null;
        }
    }

    private static bool TryReadFields(string json, out Dictionary<string, string> fields)
    {
        fields = new Dictionary<string, string>(StringComparer.Ordinal);

        try
        {
            using var document = JsonDocument.Parse(json);

            if (document.RootElement.ValueKind is not JsonValueKind.Object)
            {
                return false;
            }

            var names = new HashSet<string>(StringComparer.Ordinal);

            foreach (var property in document.RootElement.EnumerateObject())
            {
                // A parser may read a different duplicate than the one that was verified
                if (!names.Add(property.Name))
                {
                    return false;
                }

                if (property.Value.ValueKind is JsonValueKind.String)
                {
                    fields.Add(property.Name, property.Value.GetString()!);
                }
            }

            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static bool TryGetStringToSign(Dictionary<string, string> fields, string[] signedFields, out string stringToSign)
    {
        var sb = new StringBuilder();

        stringToSign = "";

        foreach (var name in signedFields)
        {
            if (fields.TryGetValue(name, out var value))
            {
                sb.Append(name).Append('\n').Append(value).Append('\n');
            }
            else if (name is not "Subject")
            {
                return false;
            }
        }

        stringToSign = sb.ToString();

        return true;
    }

    [GeneratedRegex(@"\Asns\.[a-z0-9-]+\.amazonaws\.com(\.cn)?\z")]
    private static partial Regex SnsHostRegex();
}
