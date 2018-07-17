using System;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Messaging;

namespace Amazon.Sqs
{
    using Models;

    public sealed class SqsClient : AwsClient
    {
        public const string Version = "2012-11-05";

        public const string NS = "http://queue.amazonaws.com/doc/2012-11-05/";

        public SqsClient(AwsRegion region, IAwsCredential credential)
            : base(AwsService.Sqs, region, credential)
        { }

        public async Task<CreateQueueResult> CreateQueueAsync(string queueName, int defaultVisibilityTimeout = 30)
        {
            if (queueName == null)
                throw new ArgumentNullException(nameof(queueName));

            var parameters = new SqsRequest {
                { "Action", "CreateQueue" },
                { "QueueName", queueName },
                { "DefaultVisibilityTimeout", defaultVisibilityTimeout } /* in seconds */
			};

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Content = GetPostContent(parameters)
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return CreateQueueResponse.Parse(responseText).CreateQueueResult;
        }

        /*
        public Task DeleteQueue(string queueName)
        {
            throw new NotImplementedException();
        }
        */

        public async Task<SendMessageBatchResultEntry[]> SendMessageBatchAsync(Uri queueUrl, string[] messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            if (messages.Length > 10)
                throw new ArgumentException("Must be 10 or fewer.", nameof(messages));

            // Max payload = 256KB (262,144 bytes)

            var parameters = new SqsRequest {
                { "Action", "SendMessageBatch" }
            };

            for (int i = 0; i < messages.Length; i++)
            {
                var message = messages[i];

                var prefix = "SendMessageBatchRequestEntry." + (i + 1) + ".";

                parameters.Add(prefix + "Id", i);                   // .Id				Required
                parameters.Add(prefix + "MessageBody", message);    // .MessageBody		Required
                                                                    // .DelaySeconds	Optional, Max 900(15min)

            }

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
                Content = GetPostContent(parameters)
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return SendMessageBatchResponse.Parse(responseText).SendMessageBatchResult.Items;
        }

        public async Task<SendMessageResult> SendMessageAsync(Uri queueUrl, SendMessageRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
                Content = GetPostContent(request.ToParams())
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return SendMessageResponse.Parse(responseText).SendMessageResult;
        }

        public async Task<SqsMessage[]> ReceiveMessagesAsync(Uri queueUrl, RecieveMessagesRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
                Content = GetPostContent(request.ToParams())
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            var response = ReceiveMessageResponse.Parse(responseText);

            return response.ReceiveMessageResult.Items ?? Array.Empty<SqsMessage>();
        }

        public async Task<string> DeleteMessageAsync(Uri queueUrl, string recieptHandle)
        {
            var parameters = new SqsRequest {
                { "Action", "DeleteMessage" },
                { "ReceiptHandle", recieptHandle }
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
                Content = GetPostContent(parameters)
            };

            return await SendAsync(httpRequest).ConfigureAwait(false);
        }

        public async Task<DeleteMessageBatchResultEntry[]> DeleteMessageBatchAsync(Uri queueUrl, string[] recieptHandles)
        {
            if (recieptHandles == null)
                throw new ArgumentNullException(nameof(recieptHandles));

            if (recieptHandles.Length > 10)
                throw new ArgumentException("Must contain 10 or fewer items.", nameof(recieptHandles));

            // Max payload = 64KB (65,536 bytes)

            var parameters = new SqsRequest {
                { "Action", "DeleteMessageBatch" }
            };

            for (int i = 0; i < recieptHandles.Length; i++)
            {
                var handle = recieptHandles[i];

                var prefix = "DeleteMessageBatchRequestEntry." + (i + 1) + ".";

                parameters.Add(prefix + "Id", i);                   // DeleteMessageBatchRequestEntry.n.Id
                parameters.Add(prefix + "ReceiptHandle", handle);   // DeleteMessageBatchRequestEntry.n.ReceiptHandle			Required
            }

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
                Content = GetPostContent(parameters)
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            // Because the batch request can result in a combination of successful and unsuccessful actions, 
            // you should check for batch errors even when the call returns an HTTP status code of 200.
            return DeleteMessageBatchResponse.Parse(responseText).DeleteMessageBatchResult.Items;
        }

        #region Helpers

        private static FormUrlEncodedContent GetPostContent(SqsRequest request)
        {
            request.Add("Version", Version);

            return new FormUrlEncodedContent(request.Parameters);
        }

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync();

            throw new QueueException(response.StatusCode + "/" + responseText);
        }

        #endregion
    }
}

// http://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/Welcome.html

/*
<?xml version="1.0"?>
<ErrorResponse xmlns="http://queue.amazonaws.com/doc/2012-11-05/">
	<Error>
		<Type>Sender</Type>
		<Code>SignatureDoesNotMatch</Code>
		<Message>Credential should be scoped to correct service: 'sqs'. </Message>
		<Detail/>
	</Error>
	<RequestId>a805c8c5-1bef-5b1b-a9cf-86ded9669a8c</RequestId>
</ErrorResponse>
*/
