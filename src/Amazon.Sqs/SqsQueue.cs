using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Amazon.Scheduling;

using Carbon.Messaging;

namespace Amazon.Sqs
{
    public sealed class SqsQueue : IMessageQueue<string>
    {
        private readonly SqsClient client;
        private readonly Uri url;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
            initialDelay : TimeSpan.FromMilliseconds(500),
            maxDelay     : TimeSpan.FromSeconds(5),
            maxRetries   : 3
        );

        public SqsQueue(AwsRegion region, string accountId, string queueName, IAwsCredential credential)
        {
            if (region is null)
                throw new ArgumentNullException(nameof(region));

            if (accountId is null)
                throw new ArgumentNullException(nameof(accountId));

            if (queueName is null)
                throw new ArgumentNullException(nameof(queueName));
            
            this.url = new Uri($"https://sqs.{region}.amazonaws.com/{accountId}/{queueName}");

            this.client = new SqsClient(region, credential);
        }

        public async Task<IReadOnlyList<IQueueMessage<string>>> PollAsync(
            int take, 
            TimeSpan? lockTime,
            CancellationToken cancellationToken = default)
        {
            // Blocks until we recieve a message

            var request = new RecieveMessagesRequest(take, lockTime, TimeSpan.FromSeconds(20));

            while (!cancellationToken.IsCancellationRequested)
            {
                SqsMessage[] result = await client.ReceiveMessagesAsync(url, request, cancellationToken).ConfigureAwait(false);

                if (result.Length > 0)
                {
                    return result;
                }
            }

            return Array.Empty<IQueueMessage<string>>();
        }

        public async Task<IReadOnlyList<IQueueMessage<string>>> GetAsync(
            int take, 
            TimeSpan? lockTime, 
            CancellationToken cancellationToken = default)
        {
            var request = new RecieveMessagesRequest(take, lockTime);

            int retryCount = 0;
            Exception lastError;

            do
            {
                try
                {
                    return await client.ReceiveMessagesAsync(url, request, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex) when (retryPolicy.ShouldRetry(retryCount) && ex is IException { IsTransient: true })
                {
                    lastError = ex;
                }

                retryCount++;

                await Task.Delay(retryPolicy.GetDelay(retryCount), cancellationToken).ConfigureAwait(false);
            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastError;
        }

        public async Task PutAsync(params IMessage<string>[] messages)
        {
            // Max payload = 256KB (262,144 bytes)
            // Max batch size = 10

            foreach (List<IMessage<string>> batch in messages.Batch(10))
            {
                var bodyValues = new string[batch.Count];

                for (int i = 0; i < batch.Count; i++)
                {
                    bodyValues[i] = batch[i].Body;
                }

                await client.SendMessageBatchAsync(url, bodyValues).ConfigureAwait(false);
            }
        }

        public async Task UpdateMessageVisibilityAsync(string receiptHandle, TimeSpan visibilityTimeout)
        {
            var request = new ChangeMessageVisibilityRequest(receiptHandle, visibilityTimeout);

            await client.ChangeMessageVisibilityAsync(url, request).ConfigureAwait(false);
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

            do
            {
                try
                {
                    await client.DeleteMessageBatchAsync(url, receiptHandles).ConfigureAwait(false);

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