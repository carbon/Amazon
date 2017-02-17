using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Carbon.Messaging;

namespace Amazon.Sqs
{
    using Helpers;
    using Scheduling;

    public class SqsQueue : IMessageQueue<string>
    {
        private readonly SqsClient client;
        private readonly Uri url;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
            initialDelay: TimeSpan.FromSeconds(1),
            maxDelay: TimeSpan.FromSeconds(5),
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

        // Rename PollOnce ?

        public async Task<IReadOnlyList<IQueueMessage<string>>> PollAsync(int take, TimeSpan? lockTime, CancellationToken cancelationToken)
        {
            // Blocks until we recieve a message

            while (!cancelationToken.IsCancellationRequested)
            {
                var result = await client.ReceiveMessagesAsync(url, new RecieveMessagesRequest(take, lockTime, TimeSpan.FromSeconds(20))).ConfigureAwait(false);

                if (result.Length > 0)
                {
                    return result;
                }
            }

            return Array.Empty<IQueueMessage<string>>();
        }

        public async Task<IReadOnlyList<IQueueMessage<string>>> GetAsync(int take, TimeSpan? lockTime) => 
            (await client.ReceiveMessagesAsync(url, new RecieveMessagesRequest(take, lockTime)).ConfigureAwait(false));

        public async Task PutAsync(params IMessage<string>[] messages)
        {
            // Max payload = 256KB (262,144 bytes)

            foreach (var batch in messages.Batch(10))
            {
                await client.SendMessageBatchAsync(url, batch.Select(b => b.Body).ToArray()).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(params IQueueMessage<string>[] messages)
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
                catch (Exception ex)
                {
                    // TODO: Make sure it's recoverable

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