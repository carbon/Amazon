using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;

using Carbon.Messaging;

namespace Amazon.Sqs
{
    using Models;

    public class SqsClient : AwsClient
    {
        public static string Version = "2012-11-05";

        public static readonly XNamespace NS = "http://queue.amazonaws.com/doc/2012-11-05/";

        public SqsClient(AwsRegion region, IAwsCredentials credentials)
            : base(AwsService.Sqs, region, credentials)
        { }

        public async Task<CreateQueueResult> CreateQueue(string queueName, int defaultVisibilityTimeout = 30)
        {
            var parameters = new SqsRequest {
                { "Action", "CreateQueue" },
                { "QueueName", queueName },
                { "DefaultVisibilityTimeout", defaultVisibilityTimeout } /* in seconds */
			};

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
                Content = GetPostContent(parameters)
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return CreateQueueResult.Parse(responseText);
        }

        public Task DeleteQueue(string queueName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SendMessageBatchResultEntry>> SendMessageBatch(Uri queueUrl, string[] messages)
        {
            #region Preconditions

            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            if (messages.Length > 10)
                throw new ArgumentException("Must be 10 or fewer.", "messages.Length");

            // Max payload = 256KB (262,144 bytes)

            #endregion

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

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl)
            {
                Content = GetPostContent(parameters)
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return SendMessageBatchResult.Parse(responseText);
        }

        public async Task<SendMessageResult> SendMessage(Uri queueUrl, SendMessageRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl)
            {
                Content = GetPostContent(request.ToParams())
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return SendMessageResult.Parse(responseText);
        }

        public async Task<SqsMessage[]> ReceiveMessages(Uri queueUrl, RecieveMessagesRequest request)
        {
            #region Preconditions

            if (request == null) throw new ArgumentNullException(nameof(request));

            #endregion

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl)
            {
                Content = GetPostContent(request.ToParams())
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            return RecieveMessageResponse.Parse(responseText).ToArray();
        }

        public async Task<string> DeleteMessage(Uri queueUrl, string recieptHandle)
        {
            var parameters = new SqsRequest {
                { "Action", "DeleteMessage" },
                { "ReceiptHandle", recieptHandle }
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl)
            {
                Content = GetPostContent(parameters)
            };

            return await SendAsync(httpRequest).ConfigureAwait(false);
        }

        public async Task<List<DeleteMessageBatchResultEntry>> DeleteMessageBatch(Uri queueUrl, string[] recieptHandles)
        {
            #region Preconditions

            if (recieptHandles == null) throw new ArgumentNullException(nameof(recieptHandles));

            if (recieptHandles.Length > 10) throw new ArgumentException("Must be 10 or fewer.", "messages.Length");

            // Max payload = 64KB (65,536 bytes)

            #endregion

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

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl)
            {
                Content = GetPostContent(parameters)
            };

            var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

            // Because the batch request can result in a combination of successful and unsuccessful actions, 
            // you should check for batch errors even when the call returns an HTTP status code of 200.
            return DeleteMessageBatchResult.Parse(responseText);
        }

        #region Helpers

        private FormUrlEncodedContent GetPostContent(SqsRequest request)
        {
            request.Add("Version", Version);

            return new FormUrlEncodedContent(request.Parameters);
        }

        protected override async Task<Exception> GetException(HttpResponseMessage response)
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
