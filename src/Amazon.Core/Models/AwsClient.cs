using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon
{
    using Security;

    public abstract class AwsClient : IDisposable
    {
        protected readonly HttpClient httpClient = new HttpClient(
            new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.GZip
            }
        );

        private readonly AwsService service;
        protected readonly AwsRegion region;
        private IAwsCredentials credentials;
        private readonly SemaphoreSlim mutex = new SemaphoreSlim(1);

        public AwsClient(AwsService service, AwsRegion region, IAwsCredentials credentials)
        {
            #region Preconditions

            if (credentials == null) throw new ArgumentNullException(nameof(credentials));

            #endregion

            this.service = service;
            this.region = region;
            this.credentials = credentials;

            Endpoint = $"https://{service.Name}.{region.Name}.amazonaws.com/";
        }

        public string Endpoint { get; }

        public AwsRegion Region => region;

        protected virtual async Task<Exception> GetException(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new Exception(responseText);
        }

        protected async Task<string> SendAsync(HttpRequestMessage request)
        {
            await SignAsync(request).ConfigureAwait(false);

            using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw await GetException(response).ConfigureAwait(false);
                }

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        protected async Task SignAsync(HttpRequestMessage httpRequest)
        {
            if (credentials.ShouldRenew)
            {
                await mutex.WaitAsync().ConfigureAwait(false);

                try
                {
                    credentials = await credentials.RenewAsync().ConfigureAwait(false);
                }
                finally
                {
                    mutex.Release();
                }
            }

            var date = DateTimeOffset.UtcNow;

            httpRequest.Headers.UserAgent.ParseAdd("Carbon/1.5");
            httpRequest.Headers.Host = httpRequest.RequestUri.Host;
            httpRequest.Headers.Date = date;

            if (credentials.SecurityToken != null)
            {
                httpRequest.Headers.Add("x-amz-security-token", credentials.SecurityToken);
            }

            httpRequest.Headers.Add("x-amz-date", date.UtcDateTime.ToString("yyyyMMddTHHmmssZ"));

            var scope = GetCredentialScope(httpRequest);

            SignerV4.Default.Sign(credentials, scope, httpRequest);
        }

        private CredentialScope GetCredentialScope(HttpRequestMessage httpRequest) => 
            new CredentialScope(httpRequest.Headers.Date.Value.UtcDateTime, region, service);

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}