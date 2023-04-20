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
    private readonly Uri _url;

    private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
        initialDelay : TimeSpan.FromSeconds(0.5),
        maxDelay     : TimeSpan.FromSeconds(3),
        maxRetries   : 4
    );

    public SqsQueue(AwsRegion region, string accountId, string queueName, IAwsCredential credential)
    {
        ArgumentNullException.ThrowIfNull(accountId);
        ArgumentNullException.ThrowIfNull(queueName);

        _client = new SqsClient(region, credential);
        _url = new Uri($"https://sqs.{region}.amazonaws.com/{accountId}/{queueName}");
    }

    // TODO: Overload with serializer (Default to JSON)

    public async Task<IReadOnlyList<IQueueMessage<T>>> PollAsync(
        int take, 
        TimeSpan? lockTime,
        CancellationToken cancellationToken = default)
    {
        // Blocks until we recieve a message

        var request = new ReceiveMessagesRequest(take, lockTime, waitTime: TimeSpan.FromSeconds(20));

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await _client.ReceiveMessagesAsync(_url, request, cancellationToken).ConfigureAwait(false);

            if (result.Length == 0) continue;

            return Convert(result);
        }

        return Array.Empty<IQueueMessage<T>>();
    }

    public async Task<IReadOnlyList<IQueueMessage<T>>> GetAsync(
        int take,
        TimeSpan? lockTime, 
        CancellationToken cancellationToken)
    {
        var request = new ReceiveMessagesRequest(take, lockTime);

        var result = await _client.ReceiveMessagesAsync(_url, request, cancellationToken).ConfigureAwait(false);

        return Convert(result);
    }

    private static IQueueMessage<T>[] Convert(SqsMessage[] messages)
    {
        if (messages.Length == 0)
        {
            return Array.Empty<IQueueMessage<T>>();
        }

        var result = new IQueueMessage<T>[messages.Length];

        for (int i = 0; i < messages.Length; i++)
        {
            result[i] = new JsonEncodedMessage<T>(messages[i]);
        }

        return result;
    }

    public async Task<string> PutAsync(T message, TimeSpan? delay = null)
    {
        string text = JsonSerializer.Serialize(message, jso);

        var request = new SendMessageRequest(text) {
            Delay = delay
        };

        var result = await _client.SendMessageAsync(_url, request).ConfigureAwait(false);

        return result.MessageId;
    }

    private static readonly JsonSerializerOptions jso = new () {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task PutAsync(params IMessage<T>[] messages)
    {
        ArgumentNullException.ThrowIfNull(messages);

        if (messages.Length == 0) return;

        // Max payload = 256KB (262,144 bytes)

        // Convert the message payload to JSON

        foreach (var batch in messages.Chunk(10))
        {
            var messageBatch = new string[batch.Length];

            for (int i = 0; i < batch.Length; i++)
            {
                messageBatch[i] = JsonSerializer.Serialize(batch[i].Body, jso);
            }

            await _client.SendMessageBatchAsync(_url, messageBatch).ConfigureAwait(false);
        }
    }

    public async Task UpdateMessageVisibilityAsync(string receiptHandle, TimeSpan duration)
    {
        var request = new ChangeMessageVisibilityRequest(receiptHandle, duration);

        await _client.ChangeMessageVisibilityAsync(_url, request).ConfigureAwait(false);
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

        do
        {
            try
            {
                await _client.DeleteMessageBatchAsync(_url, handles).ConfigureAwait(false);

                return;
            }
            catch (Exception ex) when (retryPolicy.ShouldRetry(retryCount) && ex is IException { IsTransient: true })
            {
                lastError = ex;
            }

            retryCount++;

            await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
        }

        while (retryPolicy.ShouldRetry(retryCount));

        throw lastError;
    }
}