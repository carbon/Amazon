using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Amazon.Security;

namespace Amazon
{
    public abstract class AwsClient : IDisposable
    {
        protected readonly HttpClient httpClient = new HttpClient(
            new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip }
        ) {
            DefaultRequestHeaders = {
                {  "User-Agent", "Carbon/2.1" }
            }
        };
        
        private readonly AwsService service;
        protected readonly IAwsCredential credential;

        public AwsClient(AwsService service, AwsRegion region, IAwsCredential credential)
        {
            this.service    = service    ?? throw new ArgumentNullException(nameof(service));
            Region          = region     ?? throw new ArgumentNullException(nameof(region));
            this.credential = credential ?? throw new ArgumentNullException(nameof(credential));

            Endpoint = $"https://{service.Name}.{region.Name}.amazonaws.com/";
        }

        public string Endpoint { get; }

        public AwsRegion Region { get; }

        protected async Task<string> SendAsync(HttpRequestMessage request)
        {
            await SignAsync(request).ConfigureAwait(false);

            using HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw await GetExceptionAsync(response).ConfigureAwait(false);
            }

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        
        protected async Task SignAsync(HttpRequestMessage request)
        {
            if (credential.ShouldRenew)
            {
                await credential.RenewAsync().ConfigureAwait(false);   
            }

            var date = DateTimeOffset.UtcNow;

            request.Headers.Host = request.RequestUri.Host;
            request.Headers.Date = date;

            if (credential.SecurityToken != null)
            {
                request.Headers.Add("x-amz-security-token", credential.SecurityToken);
            }

            request.Headers.Add("x-amz-date", date.UtcDateTime.ToString("yyyyMMddTHHmmssZ"));

            SignerV4.Default.Sign(credential, scope: GetCredentialScope(request), request: request);
        }

        protected virtual async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new Exception(responseText);
        }

        private CredentialScope GetCredentialScope(HttpRequestMessage httpRequest)
        {
            return new CredentialScope(httpRequest.Headers.Date.Value.UtcDateTime, Region, service);
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}