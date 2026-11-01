using System.Net.Http;
using System.Text;
using System.Threading;
using System.Xml;

using Amazon.Scheduling;

namespace Amazon.Sns;

public sealed class SnsClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Sns, region, credential)
{
    public const string Version = "2010-03-31";

    private static readonly RetryPolicy s_retryPolicy = RetryPolicy.ExponentialBackoff(
        initialDelay : TimeSpan.FromMilliseconds(100),
        maxDelay     : TimeSpan.FromSeconds(5),
        maxRetries   : 3
    );

    public async Task<PublishResult> PublishAsync(PublishRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var responseBytes = await SendWithRetryAsync(request.ToParams(), cancellationToken).ConfigureAwait(false);

        return PublishResult.Parse(responseBytes);
    }

    public async Task<ConfirmSubscriptionResult> ConfirmSubscriptionAsync(ConfirmSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var responseBytes = await SendWithRetryAsync(request.ToParams(), cancellationToken).ConfigureAwait(false);

        return new ConfirmSubscriptionResult(SnsXml.GetRequiredValue(responseBytes, "SubscriptionArn"));
    }

    #region Helpers

    private async Task<byte[]> SendWithRetryAsync(List<KeyValuePair<string, string>> parameters, CancellationToken cancellationToken)
    {
        parameters.Add(new("Version", Version));

        var retryCount = 0;
        Exception lastException;

        do
        {
            if (retryCount > 0)
            {
                await Task.Delay(s_retryPolicy.GetDelay(retryCount), cancellationToken).ConfigureAwait(false);
            }

            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                    Content = new FormUrlEncodedContent(parameters)
                };

                return await SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            }
            catch (SnsException ex) when (ex.IsTransient)
            {
                lastException = ex;
            }

            retryCount++;
        }
        while (s_retryPolicy.ShouldRetry(retryCount));

        throw lastException;
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        var responseBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        string? code = null;
        string? message = null;
        string? requestId = null;

        try
        {
            var root = SnsXml.Parse(responseBytes);

            code = SnsXml.GetValue(root, "Code");
            message = SnsXml.GetValue(root, "Message");
            requestId = SnsXml.GetValue(root, "RequestId");
        }
        catch (XmlException) { }

        return new SnsException(
            string.IsNullOrEmpty(message) ? Encoding.UTF8.GetString(responseBytes) : message,
            response.StatusCode,
            code,
            requestId);
    }

    #endregion
}

// http://docs.aws.amazon.com/sns/latest/api/Welcome.html
// sns:Publish	This grants permission to publish to a topic.
