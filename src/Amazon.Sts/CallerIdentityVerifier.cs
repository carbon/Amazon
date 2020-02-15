using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Amazon.Sts.Exceptions;

namespace Amazon.Sts
{
    public sealed class CallerIdentityVerifier 
    {
        private readonly HttpClient httpClient = new HttpClient {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/2.1" }
            }
        };

        public async Task<GetCallerIdentityResult> VerifyCallerIdentityAsync(CallerIdentityVerificationParameters token)
        {
            var uri = new Uri(token.Url);

            if (uri.Scheme != "https")
            {
                throw new ArgumentException("Endpoint scheme be https. Was " + uri.Scheme);
            }

            // https://sts.us-east-1.amazonaws.com/

            if (!(uri.Host.StartsWith("sts.", StringComparison.Ordinal) && uri.Host.EndsWith(".amazonaws.com", StringComparison.Ordinal)))
            {
                throw new Exception("Must be an STS endpoint: was:" + token.Url);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, token.Url) {
                Content = new StringContent(token.Body, Encoding.UTF8, "application/x-www-form-urlencoded")
            };

            foreach (var header in token.Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            request.Headers.Host = uri.Host;

            // Our message should be signed

            using HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new StsException(response.StatusCode, responseText);
            }

            return StsSerializer<GetCallerIdentityResponse>.ParseXml(responseText).GetCallerIdentityResult;
        }
    }
}