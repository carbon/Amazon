using System.Net.Http;
using System.Net.Mail;

using Amazon.Scheduling;

namespace Amazon.Ses;

public sealed class SesClient : AwsClient
{
    public const string Version = "2010-12-01";

    public const string Namespace = "http://ses.amazonaws.com/doc/2010-12-01/";

    private static readonly ExponentialBackoffRetryPolicy retryPolicy = new (
        initialDelay : TimeSpan.FromSeconds(1),
        maxDelay     : TimeSpan.FromSeconds(10),
        maxRetries   : 5
    );

    public SesClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.Ses, region, credential)
    { }

    public Task<SendEmailResult> SendEmailAsync(MailMessage message)
    {
        return SendEmailAsync(SesEmail.FromMailMessage(message));
    }

    public async Task<SendEmailResult> SendEmailAsync(SesEmail message)
    {
        var request = new SesRequest("SendEmail");

        foreach (var pair in message.ToParams())
        {
            request.Add(pair.Key, pair.Value);
        }

        var text = await SendWithRetryPolicy(request, retryPolicy).ConfigureAwait(false);

        return SendEmailResponse.Deserialize(text).SendEmailResult;
    }

    public async Task<SendRawEmailResult> SendRawEmailAsync(SendRawEmailRequest request)
    {
        var data = new SesRequest("SendRawEmail");

        foreach (var pair in request.ToParams())
        {
            data.Add(pair.Key, pair.Value);
        }

        var text = await SendWithRetryPolicy(data, retryPolicy).ConfigureAwait(false);

        return SendRawEmailResponse.Deserialize(text).SendRawEmailResult;
    }

    public async Task<GetSendQuotaResult> GetSendQuotaAsync()
    {
        var request = new SesRequest("GetSendQuota");

        var text = await PostAsync(request).ConfigureAwait(false);

        return GetSendQuotaResponse.Deserialize(text).GetSendQuotaResult;
    }

    #region Helpers

    private async Task<string> SendWithRetryPolicy(SesRequest request, RetryPolicy retryPolicy)
    {
        var retryCount = 0;
        Exception lastException;

        do
        {
            if (retryCount > 0)
            {
                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            try
            {
                return await PostAsync(request).ConfigureAwait(false);
            }
            catch (SesException ex) when (ex.IsTransient)
            {
                lastException = ex;
            }

            retryCount++;
        }
        while (retryPolicy.ShouldRetry(retryCount));

        throw lastException;
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var errorResponse = ErrorResponse.Deserialize(responseText);

        return new SesException(errorResponse.Error, response.StatusCode);
    }

    private Task<string> PostAsync(SesRequest request)
    {
        request.Parameters.Add("Version", Version);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = new FormUrlEncodedContent(request.Parameters!)
        };

        return base.SendAsync(httpRequest);
    }

    #endregion
}