using System.Linq;
using System.Threading;

using Amazon.Scheduling;

using Carbon.Messaging;

namespace Amazon.Sqs;

public sealed class SqsQueue : IMessageQueue<string>
{
    private readonly SqsClient _client;
    private readonly string _queueUrl;

    private static readonly TimeSpan s_defaultWaitTime = TimeSpan.FromSeconds(20);

    private static readonly RetryPolicy s_retryPolicy = RetryPolicy.ExponentialBackoff(
        initialDelay : TimeSpan.FromMilliseconds(500),
        maxDelay     : TimeSpan.FromSeconds(5),
        maxRetries   : 3
    );

    public SqsQueue(AwsRegion region, string accountId, string queueName, IAwsCredential credential)
    {
        ArgumentException.ThrowIfNullOrEmpty(accountId);
        ArgumentException.ThrowIfNullOrEmpty(queueName);

        _client = new SqsClient(region, credential);
        _queueUrl = $"https://sqs.{region}.amazonaws.com/{accountId}/{queueName}";
    }

    public async Task<IReadOnlyList<IQueueMessage<string>>> PollAsync(
        int take,
        TimeSpan? lockTime,
        CancellationToken cancellationToken = default)
    {
        // Blocks until we receive a message

        var request = new ReceiveMessageRequest(
            queueUrl            : _queueUrl,
            maxNumberOfMessages : take, 
            visibilityTimeout   : lockTime, 
            waitTime            : s_defaultWaitTime);

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await _client.ReceiveMessagesAsync(request, cancellationToken).ConfigureAwait(false);

            if (result.Messages is { Length: > 0 } messages)
            {
                return messages;
            }
        }

        return [];
    }

    public async Task<IReadOnlyList<IQueueMessage<string>>> GetAsync(
        int take,
        TimeSpan? lockTime,
        CancellationToken cancellationToken = default)
    {
        var request = new ReceiveMessageRequest(
            queueUrl            : _queueUrl,
            maxNumberOfMessages : take, 
            visibilityTimeout   : lockTime
        );

        int retryCount = 0;
        Exception lastError;

        do
        {
            try
            {
                var result = await _client.ReceiveMessagesAsync(request, cancellationToken).ConfigureAwait(false);

                return result.Messages ?? [];
            }
            catch (Exception ex) when (s_retryPolicy.ShouldRetry(retryCount) && ex is IException { IsTransient: true })
            {
                lastError = ex;
            }

            retryCount++;

            await Task.Delay(s_retryPolicy.GetDelay(retryCount), cancellationToken).ConfigureAwait(false);
        }
        while (s_retryPolicy.ShouldRetry(retryCount));

        throw lastError;
    }

    public async Task PutAsync(params IMessage<string>[] messages)
    {
        // Max payload = 256KB (262,144 bytes)
        // Max batch size = 10

        foreach (IMessage<string>[] batch in messages.Chunk(10))
        {
            var bodyValues = new string[batch.Length];

            for (int i = 0; i < batch.Length; i++)
            {
                bodyValues[i] = batch[i].Body;
            }

            await _client.SendMessageBatchAsync(new SendMessageBatchRequest(_queueUrl, bodyValues)).ConfigureAwait(false);
        }
    }

    public async Task UpdateMessageVisibilityAsync(string receiptHandle, TimeSpan visibilityTimeout)
    {
        await _client.ChangeMessageVisibilityAsync(new(
            queueUrl          : _queueUrl,
            receiptHandle     : receiptHandle,
            visibilityTimeout : visibilityTimeout
        )).ConfigureAwait(false);
    }

    public async Task DeleteAsync(params IQueueMessage<string>[] messages)
    {
        var receiptHandles = new string[messages.Length];

        for (int i = 0; i < messages.Length; i++)
        {
            receiptHandles[i] = messages[i].Receipt.Handle;
        }

        int retryCount = 0;
        Exception lastError;

        var request = new DeleteMessageBatchRequest(_queueUrl, receiptHandles);

        do
        {
            try
            {
                await _client.DeleteMessageBatchAsync(request).ConfigureAwait(false);

                return;
            }
            catch (Exception ex) when (s_retryPolicy.ShouldRetry(retryCount) && ex is IException { IsTransient: true })
            {
                lastError = ex;
            }

            retryCount++;

            await Task.Delay(s_retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
        }
        while (s_retryPolicy.ShouldRetry(retryCount));

        throw lastError;
    }
}