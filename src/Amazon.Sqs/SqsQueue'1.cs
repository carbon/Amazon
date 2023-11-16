using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

using Amazon.Scheduling;

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

            var messages = result.Messages;

            if (messages.Length is 0) continue;

            return Convert(messages);
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

        // Max payload = 256KB (262,144 bytes)

        // Convert the message payload to JSON

        foreach (var batch in messages.Chunk(10))
        {
            var messageBatch = new string[batch.Length];

            for (int i = 0; i < batch.Length; i++)
            {
                messageBatch[i] = JsonSerializer.Serialize(batch[i].Body, s_jso);
            }

            await _client.SendMessageBatchAsync(new SendMessageBatchRequest(_queueUrl, messageBatch)).ConfigureAwait(false);
        }
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

        var retryCount = 0;
        Exception lastError;

        var request = new DeleteMessageBatchRequest(_queueUrl, handles);

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