using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

using Amazon.Scheduling;
using Amazon.Sqs.Exceptions;

using Carbon.Messaging;

namespace Amazon.Sqs;

public sealed class SqsQueue<T> : IMessageQueue<T>
    where T : notnull, new()
{
    private readonly SqsClient _client;
    private readonly string _queueUrl;

    private static readonly RetryPolicy s_retryPolicy = RetryPolicy.ExponentialBackoff(
        initialDelay : TimeSpan.FromSeconds(0.5),
        maxDelay     : TimeSpan.FromSeconds(3),
        maxRetries   : 4
    );

    public SqsQueue(AwsRegion region, string accountId, string queueName, IAwsCredential credential)
    {
        ArgumentException.ThrowIfNullOrEmpty(accountId);
        ArgumentException.ThrowIfNullOrEmpty(queueName);

        _client   = new SqsClient(region, credential);
        _queueUrl = $"https://sqs.{region}.amazonaws.com/{accountId}/{queueName}";
    }

    // TODO: Overload with serializer (Default to JSON)

    public async Task<IReadOnlyList<IQueueMessage<T>>> PollAsync(
        int take, 
        TimeSpan? lockTime,
        CancellationToken cancellationToken = default)
    {
        // Blocks until we receive a message

        var request = new ReceiveMessageRequest(
            queueUrl            : _queueUrl,
            maxNumberOfMessages : take,
            visibilityTimeout   : lockTime, 
            waitTime            : TimeSpan.FromSeconds(20)
        );

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await _client.ReceiveMessagesAsync(request, cancellationToken).ConfigureAwait(false);

            if (result.Messages is { Length: > 0 } messages)
            {
                return Convert(messages);
            }
        }

        return [];
    }

    public async Task<IReadOnlyList<IQueueMessage<T>>> GetAsync(
        int take,
        TimeSpan? lockTime, 
        CancellationToken cancellationToken = default)
    {
        var result = await _client.ReceiveMessagesAsync(new(
            queueUrl            : _queueUrl,
            maxNumberOfMessages : take,
            visibilityTimeout   : lockTime
        ), cancellationToken).ConfigureAwait(false);

        return Convert(result.Messages);
    }

    public async Task<string> PutAsync(T message, TimeSpan? delay = null)
    {
        string text = JsonSerializer.Serialize(message, s_jso);

        var result = await _client.SendMessageAsync(new(
            queueUrl    : _queueUrl,
            messageBody : text,
            delay       : delay
        )).ConfigureAwait(false);

        return result.MessageId;
    }

    private static readonly JsonSerializerOptions s_jso = new () {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task PutAsync(params IMessage<T>[] messages)
    {
        ArgumentNullException.ThrowIfNull(messages);

        if (messages.Length is 0) return;

        // Max payload = 1 MiB (1,048,576 bytes)

        // Convert the message payload to JSON

        foreach (var batch in messages.Chunk(10))
        {
            var messageBatch = new string[batch.Length];

            for (int i = 0; i < batch.Length; i++)
            {
                messageBatch[i] = JsonSerializer.Serialize(batch[i].Body, s_jso);
            }

            var result = await _client.SendMessageBatchAsync(new SendMessageBatchRequest(_queueUrl, messageBatch)).ConfigureAwait(false);

            if (result.Failed is { Length: > 0 } failed)
            {
                throw new SqsBatchException(failed);
            }
        }
    }

    public async Task<string> PutAsync(T message, SqsSendOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var result = await _client.SendMessageAsync(new(
            queueUrl               : _queueUrl,
            messageBody            : JsonSerializer.Serialize(message, s_jso),
            delay                  : options.Delay,
            messageDeduplicationId : options.MessageDeduplicationId,
            messageGroupId         : options.MessageGroupId
        )).ConfigureAwait(false);

        return result.MessageId;
    }

    /// <param name="messages">The messages to send, in order</param>
    /// <param name="getOptions">Selects the MessageGroupId, MessageDeduplicationId or Delay of a message</param>
    /// <exception cref="SqsBatchException">SQS rejected entries. Later batches were not sent.</exception>
    public Task PutAsync(IReadOnlyList<IMessage<T>> messages, Func<IMessage<T>, SqsSendOptions> getOptions)
    {
        ArgumentNullException.ThrowIfNull(messages);
        ArgumentNullException.ThrowIfNull(getOptions);

        var entries = new SendMessageBatchRequestEntry[messages.Count];

        for (int i = 0; i < entries.Length; i++)
        {
            var options = getOptions(messages[i]);

            entries[i] = new SendMessageBatchRequestEntry(
                id                     : ((i % 10) + 1).ToString(CultureInfo.InvariantCulture),
                messageBody            : JsonSerializer.Serialize(messages[i].Body, s_jso),
                delay                  : options.Delay,
                messageDeduplicationId : options.MessageDeduplicationId,
                messageGroupId         : options.MessageGroupId
            );
        }

        return _client.SendInBatchesAsync(_queueUrl, entries);
    }

    public async Task UpdateMessageVisibilityAsync(string receiptHandle, TimeSpan duration)
    {
        await _client.ChangeMessageVisibilityAsync(new(
            queueUrl          : _queueUrl,
            receiptHandle     : receiptHandle,
            visibilityTimeout : duration)
        ).ConfigureAwait(false);
    }

    public async Task DeleteAsync(params IQueueMessage<T>[] messages)
    {
        ArgumentNullException.ThrowIfNull(messages);

        if (messages.Length == 0) return;

        var handles = new string[messages.Length]; 

        for (int i = 0; i < messages.Length; i++)
        {
            handles[i] = messages[i].Receipt.Handle;
        }

        foreach (string[] batch in handles.Chunk(10))
        {
            await DeleteBatchAsync(batch).ConfigureAwait(false);
        }
    }

    private async Task DeleteBatchAsync(string[] handles)
    {
        var retryCount = 0;

        var request = new DeleteMessageBatchRequest(_queueUrl, handles);

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

    private static JsonEncodedMessage<T>[] Convert(ReadOnlySpan<SqsMessage> messages)
    {
        if (messages.IsEmpty)
        {
            return [];
        }

        var result = new JsonEncodedMessage<T>[messages.Length];

        for (int i = 0; i < messages.Length; i++)
        {
            result[i] = new JsonEncodedMessage<T>(messages[i]);
        }

        return result;
    }
}