using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

using Amazon.Sts.Exceptions;
using Amazon.Sts.Serialization;

namespace Amazon.Sts;

public sealed partial class CallerIdentityVerifier 
{
    private readonly HttpClient _httpClient = new () {
        DefaultRequestHeaders = {
            { "User-Agent", "Carbon/4" }
        },
        Timeout = TimeSpan.FromSeconds(30)
    };
                
    public async Task<GetCallerIdentityResult> VerifyCallerIdentityAsync(CallerIdentityVerificationParameters token)
    {
        var url = new Uri(token.Url);

        if (url.Scheme is not "https")
        {
            throw new ArgumentException($"Endpoint scheme be https. Was {url.Scheme}");
        }

        if (!StsHostRegex().IsMatch(url.Host))
        {
            throw new Exception($"Must be an STS endpoint: was:{token.Url}");
        }

        var request = new HttpRequestMessage(HttpMethod.Post, url) {
            Content = new StringContent(token.Body, Encoding.UTF8, "application/x-www-form-urlencoded")
        };

        foreach (var header in token.Headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        request.Headers.Host = url.Host;

        // Our message should be signed

        using HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);

        byte[] responseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new StsException(Encoding.UTF8.GetString(responseBytes), response.StatusCode);
        }

        return StsXmlSerializer<GetCallerIdentityResponse>.Deserialize(responseBytes).GetCallerIdentityResult;
    }

    // sts.amazonaws.com or sts.{region}.amazonaws.com
    // The region is matched by shape, since other AWS services host customer names under amazonaws.com (e.g. sts.s3.amazonaws.com is an S3 bucket)
    [GeneratedRegex(@"\Asts(\.[a-z]{2}(-[a-z]+)+-[0-9]+)?\.amazonaws\.com\z", RegexOptions.CultureInvariant)]
    private static partial Regex StsHostRegex();
}

// NOTES -------------------------------------------------------------------------------------
// Endpoint: https://sts.us-east-1.amazonaws.com/