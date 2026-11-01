using System.Globalization;
using System.Linq;
using System.Threading;

using Amazon.Scheduling;
using Amazon.Sqs.Exceptions;

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
        ) {
            // Lets a FIFO queue return the same messages if a retry follows a lost response
            ReceiveRequestAttemptId = _queueUrl.EndsWith(".fifo", StringComparison.Ordinal) ? Guid.NewGuid().ToString("N") : null
        };

        int retryCount = 0;

        while (true)
        {
            try
            {
                var result = await _client.ReceiveMessagesAsync(request, cancellationToken).ConfigureAwait(false);

                return result.Messages ?? [];
            }
            catch (Exception ex) when (s_retryPolicy.ShouldRetry(retryCount + 1) && ex is IException { IsTransient: true })
            {
                // The final attempt doesn't match this filter, so its exception reaches the caller
            }

            retryCount++;

            await Task.Delay(s_retryPolicy.GetDelay(retryCount), cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task PutAsync(params IMessage<string>[] messages)
    {
        // Max payload = 1 MiB (1,048,576 bytes)
        // Max batch size = 10

        foreach (IMessage<string>[] batch in messages.Chunk(10))
        {
            var bodyValues = new string[batch.Length];

            for (int i = 0; i < batch.Length; i++)
            {
                bodyValues[i] = batch[i].Body;
            }

            var result = await _client.SendMessageBatchAsync(new SendMessageBatchRequest(_queueUrl, bodyValues)).ConfigureAwait(false);

            if (result.Failed is { Length: > 0 } failed)
            {
                throw new SqsBatchException(failed);
            }
        }
    }

    public async Task<string> PutAsync(string message, SqsSendOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var result = await _client.SendMessageAsync(new(
            queueUrl               : _queueUrl,
            messageBody            : message,
            delay                  : options.Delay,
            messageDeduplicationId : options.MessageDeduplicationId,
            messageGroupId         : options.MessageGroupId
        )).ConfigureAwait(false);

        return result.MessageId;
    }

    /// <param name="messages">The messages to send, in order</param>
    /// <param name="getOptions">Selects the MessageGroupId, MessageDeduplicationId or Delay of a message</param>
    /// <exception cref="SqsBatchException">SQS rejected entries. Later batches were not sent.</exception>
    public Task PutAsync(IReadOnlyList<IMessage<string>> messages, Func<IMessage<string>, SqsSendOptions> getOptions)
    {
        ArgumentNullException.ThrowIfNull(messages);
        ArgumentNullException.ThrowIfNull(getOptions);

        var entries = new SendMessageBatchRequestEntry[messages.Count];

        for (int i = 0; i < entries.Length; i++)
        {
            var options = getOptions(messages[i]);

            entries[i] = new SendMessageBatchRequestEntry(
                id                     : ((i % 10) + 1).ToString(CultureInfo.InvariantCulture),
                messageBody            : messages[i].Body,
                delay                  : options.Delay,
                messageDeduplicationId : options.MessageDeduplicationId,
                messageGroupId         : options.MessageGroupId
            );
        }

        return _client.SendInBatchesAsync(_queueUrl, entries);
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
        ArgumentNullException.ThrowIfNull(messages);

        if (messages.Length is 0) return;

        var receiptHandles = new string[messages.Length];

        for (int i = 0; i < messages.Length; i++)
        {
            receiptHandles[i] = messages[i].Receipt.Handle;
        }

        foreach (string[] batch in receiptHandles.Chunk(10))
        {
            await DeleteBatchAsync(batch).ConfigureAwait(false);
        }
    }

    private async Task DeleteBatchAsync(string[] receiptHandles)
    {
        int retryCount = 0;

        var request = new DeleteMessageBatchRequest(_queueUrl, receiptHandles);

        while (true)
        {
            try
            {
                var result = await _client.DeleteMessageBatchAsync(request).ConfigureAwait(false);

                if (result.Failed is { Length: > 0 } failed)
                {
                    throw new SqsBatchException(failed);
                }

                return;
            }
            catch (Exception ex) when (s_retryPolicy.ShouldRetry(retryCount + 1) && ex is IException { IsTransient: true })
            {
                // The final attempt doesn't match this filter, so its exception reaches the caller
            }

            retryCount++;

            await Task.Delay(s_retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
        }
    }
}