using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.Sts.Tests;

internal static class FakeSts
{
    public const string AccessKeyId = "FAKEACCESSKEYID";
    public const string SecretAccessKey = "fake-secret-access-key";

    public static readonly AwsCredential Credential = new(AccessKeyId, SecretAccessKey);

    // The client has no constructor that takes an HttpClient
    public static StsClient Client(FakeStsHandler handler, AwsRegion? region = null, IAwsCredential? credential = null)
    {
        var client = new StsClient(region ?? AwsRegion.USEast1, credential ?? Credential);

        Use(client, handler);

        return client;
    }

    public static void Use(StsClient client, HttpMessageHandler handler)
    {
        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));
    }

    public static CallerIdentityVerifier Verifier(FakeStsHandler handler)
    {
        var verifier = new CallerIdentityVerifier();

        typeof(CallerIdentityVerifier)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(verifier, new HttpClient(handler));

        return verifier;
    }

    public static Func<HttpResponseMessage> Xml(string xml, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return () => new HttpResponseMessage(statusCode) {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes(xml))
        };
    }

    public static string AssumeRoleXml(DateTime expiration, string accessKeyId = "FAKETEMPKEYID", string secret = "fake-temp-secret", string token = "fake-temp-token")
    {
        return $"""
            <AssumeRoleResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
              <AssumeRoleResult>
                <AssumedRoleUser>
                  <Arn>arn:aws:sts::123456789012:assumed-role/demo/session</Arn>
                  <AssumedRoleId>FAKEROLEID:session</AssumedRoleId>
                </AssumedRoleUser>
                <Credentials>
                  <AccessKeyId>{accessKeyId}</AccessKeyId>
                  <SecretAccessKey>{secret}</SecretAccessKey>
                  <SessionToken>{token}</SessionToken>
                  <Expiration>{expiration:yyyy-MM-ddTHH:mm:ssZ}</Expiration>
                </Credentials>
              </AssumeRoleResult>
            </AssumeRoleResponse>
            """;
    }

    public const string CallerIdentityXml =
        """
        <GetCallerIdentityResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
          <GetCallerIdentityResult>
            <Arn>arn:aws:iam::123456789012:user/fake</Arn>
            <UserId>FAKEUSERID</UserId>
            <Account>123456789012</Account>
          </GetCallerIdentityResult>
          <ResponseMetadata>
            <RequestId>01234567-89ab-cdef-0123-456789abcdef</RequestId>
          </ResponseMetadata>
        </GetCallerIdentityResponse>
        """;

    public const string AccessDeniedXml =
        """
        <ErrorResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
          <Error>
            <Type>Sender</Type>
            <Code>AccessDenied</Code>
            <Message>Not authorized to perform sts:AssumeRole</Message>
          </Error>
          <RequestId>01234567-89ab-cdef-0123-456789abcdef</RequestId>
        </ErrorResponse>
        """;

    // An independent SigV4 calculation, so that these tests don't take SignerV4's word for it:
    // https://docs.aws.amazon.com/IAM/latest/UserGuide/reference_sigv-create-signed-request.html
    public static string ComputeSignature(string secretAccessKey, string credentialScope, string signedHeaders, IReadOnlyDictionary<string, string> headers, string body)
    {
        var canonicalHeaders = new StringBuilder();

        foreach (string name in signedHeaders.Split(';'))
        {
            canonicalHeaders.Append(name).Append(':').Append(headers[name]).Append('\n');
        }

        string canonicalRequest = $"POST\n/\n\n{canonicalHeaders}\n{signedHeaders}\n{Sha256Hex(body)}";

        string stringToSign = $"AWS4-HMAC-SHA256\n{headers["x-amz-date"]}\n{credentialScope}\n{Sha256Hex(canonicalRequest)}";

        byte[] key = Encoding.UTF8.GetBytes("AWS4" + secretAccessKey);

        foreach (string part in credentialScope.Split('/'))
        {
            key = HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(part));
        }

        return Convert.ToHexStringLower(HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(stringToSign)));
    }

    private static string Sha256Hex(string text)
    {
        return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(text)));
    }
}

// Authorization: AWS4-HMAC-SHA256 Credential={id}/{scope},SignedHeaders={names},Signature={signature}
internal sealed class ParsedAuthorization
{
    public ParsedAuthorization(string authorization)
    {
        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=", authorization);

        var parts = authorization.Substring("AWS4-HMAC-SHA256 ".Length).Split(',');

        Assert.Equal(3, parts.Length);

        string credential = parts[0].Substring("Credential=".Length);

        int slashIndex = credential.IndexOf('/');

        AccessKeyId = credential.Substring(0, slashIndex);
        Scope = credential.Substring(slashIndex + 1);
        SignedHeaders = parts[1].Substring("SignedHeaders=".Length);
        Signature = parts[2].Substring("Signature=".Length);
    }

    public string AccessKeyId { get; }

    public string Scope { get; }

    public string SignedHeaders { get; }

    public string Signature { get; }
}

internal sealed class RecordedStsRequest
{
    public required HttpMethod Method { get; init; }

    public required Uri RequestUri { get; init; }

    public required string Body { get; init; }

    public required string? ContentType { get; init; }

    public required Dictionary<string, string> Headers { get; init; }

    public Dictionary<string, string> Form => Body.Split('&').Select(static pair => pair.Split('=')).ToDictionary(
        static pair => WebUtility.UrlDecode(pair[0]),
        static pair => WebUtility.UrlDecode(pair[1])
    );
}

internal sealed class FakeStsHandler(params Func<HttpResponseMessage>[] responses) : HttpMessageHandler
{
    public List<RecordedStsRequest> Requests { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var header in request.Headers.NonValidated)
        {
            headers[header.Key] = header.Value.ToString();
        }

        Requests.Add(new RecordedStsRequest {
            Method      = request.Method,
            RequestUri  = request.RequestUri!,
            Body        = await request.Content!.ReadAsStringAsync(cancellationToken),
            ContentType = request.Content.Headers.ContentType?.ToString(),
            Headers     = headers
        });

        return responses[Math.Min(Requests.Count, responses.Length) - 1]();
    }
}
