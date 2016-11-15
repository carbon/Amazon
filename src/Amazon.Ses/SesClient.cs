using System;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Amazon.Ses
{
    using Scheduling;

    public class SesClient : AwsClient
    {
        public static string Version = "2010-12-01";

        private readonly RetryPolicy retryPolicy = new ExponentialBackoffRetryPolicy(
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(10),
            5
        );

        public SesClient(IAwsCredentials credentials)
             : base(AwsService.Ses, AwsRegion.USEast1, credentials)
        { }

        public Task<SendEmailResult> SendEmail(MailMessage message)
        {
            return SendEmail(SesEmail.FromMailMessage(message));
        }

        public async Task<SendEmailResult> SendEmail(SesEmail message)
        {
            var request = new SesRequest("SendEmail");

            foreach (var pair in message.ToParams())
            {
                request.Add(pair.Key, pair.Value);
            }

            var text = await SendWithRetryPolicy(request, retryPolicy).ConfigureAwait(false);

            return SendEmailResponse.Parse(text).SendEmailResult;
        }

        public async Task<GetSendQuotaResult> GetSendQuota(SesEmail message)
        {
            var request = new SesRequest("GetSendQuota");

            var text = await Send(request).ConfigureAwait(false);

            return GetSendQuotaResponse.Parse(text).GetSendQuotaResult;
        }

        #region Helpers

        private async Task<string> SendWithRetryPolicy(SesRequest request, RetryPolicy retryPolicy)
        {
            var retryCount = 0;
            Exception lastError = null;

            do
            {
                try
                {
                    return await Send(request).ConfigureAwait(false);
                }
                catch (SesException ex) when (ex.IsTransient)
                {
                    lastError = ex;
                }

                retryCount++;

                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastError;
        }

        protected override async Task<Exception> GetException(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var errorResponse = ErrorResponse.Parse(responseText);

            return new SesException(errorResponse.Error);
        }

        private Task<string> Send(SesRequest request)
        {
            request.Parameters.Add("Version", Version);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Content = new FormUrlEncodedContent(request.Parameters)
            };

            return SendAsync(httpRequest);
        }

        #endregion
    }
}