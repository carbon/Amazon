using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using Amazon.Scheduling;

using Carbon.Messaging;

namespace Amazon.Sqs
{
    public sealed class SqsQueue<T> : IMessageQueue<T>
        where T : notnull, new()
    {
        private readonly SqsClient client;
        private readonly Uri url;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
            initialDelay : TimeSpan.FromSeconds(0.5),
            maxDelay     : TimeSpan.FromSeconds(3),
            maxRetries   : 4
        );

        public SqsQueue(AwsRegion region, string accountId, string queueName, IAwsCredential credential)
        {
            if (accountId is null)
                throw new ArgumentNullException(nameof(accountId));

            if (queueName is null)
                throw new ArgumentNullException(nameof(queueName));

            this.url = new Uri($"https://sqs.{region}.amazonaws.com/{accountId}/{queueName}");

            this.client = new SqsClient(region, credential);
        }

        // TODO: Overload with serializer (Default to JSON)

        public async Task<IReadOnlyList<IQueueMessage<T>>> PollAsync(
            int take, 
            TimeSpan? lockTime,
            CancellationToken cancellationToken = default)
        {
            // Blocks until we recieve a message

            var request = new RecieveMessagesRequest(take, lockTime, waitTime: TimeSpan.FromSeconds(20));

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await client.ReceiveMessagesAsync(url, request, cancellationToken).ConfigureAwait(false);

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
            var request = new RecieveMessagesRequest(take, lockTime);

            var result = await client.ReceiveMessagesAsync(url, request, cancellationToken).ConfigureAwait(false);

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

            var result = await client.SendMessageAsync(url, request).ConfigureAwait(false);

            return result.MessageId;
        }

        private static readonly JsonSerializerOptions jso = new () {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task PutAsync(params IMessage<T>[] messages)
        {
            if (messages is null) throw new ArgumentNullException(nameof(messages));

            // Max payload = 256KB (262,144 bytes)

            // Convert the message payload to JSON

            foreach (var batch in messages.Chunk(10))
            {
                string[] messageBatch = new string[batch.Count];

                for (int i = 0; i < batch.Count; i++)
                {
                    messageBatch[i] = JsonSerializer.Serialize(batch[i].Body, jso);
                }

                await client.SendMessageBatchAsync(url, messageBatch).ConfigureAwait(false);
            }
        }

        public async Task UpdateMessageVisibilityAsync(string receiptHandle, TimeSpan duration)
        {
            var request = new ChangeMessageVisibilityRequest(receiptHandle, duration);

            await client.ChangeMessageVisibilityAsync(url, request).ConfigureAwait(false);
        }

        public async Task DeleteAsync(params IQueueMessage<T>[] messages)
        {
            if (messages is null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

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
                    await client.DeleteMessageBatchAsync(url, handles).ConfigureAwait(false);

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
}