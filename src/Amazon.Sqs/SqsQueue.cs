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
        private readonly SqsClient _client;
        private readonly Uri _url;

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

            _client = new SqsClient(region, credential);
            _url = new Uri($"https://sqs.{region}.amazonaws.com/{accountId}/{queueName}");
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
                SqsMessage[] result = await _client.ReceiveMessagesAsync(_url, request, cancellationToken).ConfigureAwait(false);

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
                    return await _client.ReceiveMessagesAsync(_url, request, cancellationToken).ConfigureAwait(false);
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

            foreach (IMessage<string>[] batch in messages.Chunk(10))
            {
                var bodyValues = new string[batch.Length];

                for (int i = 0; i < batch.Length; i++)
                {
                    bodyValues[i] = batch[i].Body;
                }

                await _client.SendMessageBatchAsync(_url, bodyValues).ConfigureAwait(false);
            }
        }

        public async Task UpdateMessageVisibilityAsync(string receiptHandle, TimeSpan visibilityTimeout)
        {
            var request = new ChangeMessageVisibilityRequest(receiptHandle, visibilityTimeout);

            await _client.ChangeMessageVisibilityAsync(_url, request).ConfigureAwait(false);
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
                    await _client.DeleteMessageBatchAsync(_url, receiptHandles).ConfigureAwait(false);

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