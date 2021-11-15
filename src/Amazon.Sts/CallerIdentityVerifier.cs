using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Amazon.Sts.Exceptions;

namespace Amazon.Sts;

public sealed class CallerIdentityVerifier 
{
    private readonly HttpClient httpClient = new () {
        DefaultRequestHeaders = {
            { "User-Agent", "Carbon/3" }
        }
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

        using HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw new StsException(responseText, response.StatusCode);
        }

        return StsSerializer<GetCallerIdentityResponse>.ParseXml(responseText).GetCallerIdentityResult;
    }
}

// NOTES -------------------------------------------------------------------------------------
// Endpoint: https://sts.us-east-1.amazonaws.com/