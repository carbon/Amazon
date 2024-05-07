using System.Net.Http;
using System.Net.Mail;

using Amazon.Scheduling;

namespace Amazon.Ses;

public sealed class SesClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Ses, region, credential)
{
    public const string Version = "2010-12-01";

    public const string Namespace = "http://ses.amazonaws.com/doc/2010-12-01/";

    private static readonly ExponentialBackoffRetryPolicy s_retryPolicy = new(
        initialDelay : TimeSpan.FromSeconds(1),
        maxDelay     : TimeSpan.FromSeconds(10),
        maxRetries   : 5
    );

    public Task<SendEmailResult> SendEmailAsync(MailMessage message)
    {
        return SendEmailAsync(SesEmail.FromMailMessage(message));
    }

    public async Task<SendEmailResult> SendEmailAsync(SesEmail message)
    {
        var request = new SesRequest("SendEmail");

        foreach (var pair in message.ToParameters())
        {
            request.Add(pair);
        }

        byte[] responseBytes = await SendWithRetryPolicy(request, s_retryPolicy).ConfigureAwait(false);

        return SendEmailResponse.Deserialize(responseBytes).SendEmailResult;
    }

    public async Task<SendRawEmailResult> SendRawEmailAsync(SendRawEmailRequest request)
    {
        var data = new SesRequest("SendRawEmail");

        foreach (var pair in request.ToParams())
        {
            data.Add(pair);
        }

        byte[] responseBytes = await SendWithRetryPolicy(data, s_retryPolicy).ConfigureAwait(false);

        return SendRawEmailResponse.Deserialize(responseBytes).SendRawEmailResult;
    }

    public async Task<GetSendQuotaResult> GetSendQuotaAsync()
    {
        var request = new SesRequest("GetSendQuota");

        byte[] responseBytes = await PostAsync(request).ConfigureAwait(false);

        return GetSendQuotaResponse.Deserialize(responseBytes).GetSendQuotaResult;
    }

    #region Helpers

    private async Task<byte[]> SendWithRetryPolicy(SesRequest request, RetryPolicy retryPolicy)
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
        var responseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        var errorResponse = ErrorResponse.Deserialize(responseBytes);

        return new SesException(errorResponse.Error, response.StatusCode);
    }

    private Task<byte[]> PostAsync(SesRequest request)
    {
        request.Parameters.Add(new("Version", Version));

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = new FormUrlEncodedContent(request.Parameters!)
        };

        return base.SendAsync(httpRequest);
    }

    #endregion
}