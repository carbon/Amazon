using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Json;
using Carbon.Messaging;

namespace Amazon.Sqs
{
    using Helpers;
    using Scheduling;

    public class SqsQueue<T> : IMessageQueue<T>
        where T : new()
    {
        private readonly SqsClient client;
        private readonly Uri url;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
            initialDelay: TimeSpan.FromSeconds(0.5),
            maxDelay: TimeSpan.FromSeconds(3),
            maxRetries: 3);

        public SqsQueue(AwsRegion region, string accountId, string queueName, IAwsCredentials credentials)
        {
            #region Preconditions

            if (accountId == null)
                throw new ArgumentNullException(nameof(accountId));

            if (queueName == null)
                throw new ArgumentNullException(nameof(queueName));

            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            #endregion

            this.url = new Uri($"https://sqs.{region}.amazonaws.com/{accountId}/{queueName}");

            this.client = new SqsClient(region, credentials);
        }

        // TODO: Overload with serializer (Default to JSON)

        public async Task<IReadOnlyList<IQueueMessage<T>>> PollAsync(int take, TimeSpan? lockTime, CancellationToken cancelationToken)
        {
            // Blocks until we recieve a message

            while (!cancelationToken.IsCancellationRequested)
            {
                var result = await client.ReceiveMessagesAsync(url, new RecieveMessagesRequest(take, lockTime, TimeSpan.FromSeconds(20))).ConfigureAwait(false);

                if (result.Length == 0) continue;

                return result.Select(m => (IQueueMessage<T>)new JsonEncodedMessage<T>(m)).ToList();
            }

            return new IQueueMessage<T>[0];
        }

        public async Task<IReadOnlyList<IQueueMessage<T>>> GetAsync(int take, TimeSpan? lockTime)
        {
            var request = new RecieveMessagesRequest(take, lockTime);

            return (await client.ReceiveMessagesAsync(url, request).ConfigureAwait(false))
                .Select(m => (IQueueMessage<T>)new JsonEncodedMessage<T>(m))
                .ToList();
        }

        public Task PutAsync(T message, TimeSpan? delay = null)
        {
            var serializer = new JsonSerializer();

            var text = serializer.Serialize(message).ToString(pretty: false);

            return client.SendMessageAsync(url, new SendMessageRequest(text) { Delay = delay });
        }

        public async Task PutAsync(params IMessage<T>[] messages)
        {
            // Max payload = 256KB (262,144 bytes)

            // Convert the message payload to JSON

            var serializer = new JsonSerializer();

            foreach (var batch in messages.Batch(10))
            {
                var messageBatch = batch.Select(m =>
                    serializer.Serialize(m.Body).ToString(pretty: false)
                ).ToArray();

                await client.SendMessageBatchAsync(url, messageBatch).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(params IQueueMessage<T>[] messages)
        {
            var handles = messages.Select(m => m.Receipt.Handle).ToArray();

            var retryCount = 0;
            Exception lastError = null;

            do
            {
                try
                {
                    await client.DeleteMessageBatchAsync(url, handles).ConfigureAwait(false);

                    return;
                }
                catch (Exception ex) // TODO: Make sure it's recoverable
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

    public class JsonEncodedMessage<T> : IQueueMessage<T>
            where T : new()
    {
        private readonly SqsMessage model;

        public JsonEncodedMessage(SqsMessage model)
        {
            this.model = model;

            Body = JsonObject.Parse(model.Body).As<T>();
        }

        public static JsonEncodedMessage<T> Create(SqsMessage message)
            => new JsonEncodedMessage<T>(message);

        public string Id => model.Id;

        public MessageReceipt Receipt => model.Receipt;

        public T Body { get; }
    }
}