using System.Net.Http;
using System.Text;

using Amazon.Sts.Exceptions;
using Amazon.Sts.Serialization;

namespace Amazon.Sts;

public sealed class CallerIdentityVerifier 
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

        if (!(url.Host.StartsWith("sts.", StringComparison.Ordinal) && url.Host.EndsWith(".amazonaws.com", StringComparison.Ordinal)))
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
}

// NOTES -------------------------------------------------------------------------------------
// Endpoint: https://sts.us-east-1.amazonaws.com/