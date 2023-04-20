﻿using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;

using Amazon.Exceptions;
using Amazon.Sqs.Exceptions;
using Amazon.Sqs.Models;

using Carbon.Messaging;

namespace Amazon.Sqs;

public sealed class SqsClient : AwsClient
{
    public const string Version = "2012-11-05";

    public const string NS = "http://queue.amazonaws.com/doc/2012-11-05/";

    public SqsClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.Sqs, region, credential)
    { }

    public async Task<CreateQueueResult> CreateQueueAsync(string queueName, int defaultVisibilityTimeout = 30)
    {
        ArgumentNullException.ThrowIfNull(queueName);

        var parameters = new List<KeyValuePair<string, string>>(4) {
            new ("Action", "CreateQueue"),
            new ("QueueName", queueName),
            new ("DefaultVisibilityTimeout", defaultVisibilityTimeout.ToString(CultureInfo.InvariantCulture)) /* in seconds */
        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(parameters)
        };

        var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return CreateQueueResponse.Deserialize(responseText).CreateQueueResult;
    }

    /*
    public Task DeleteQueueAsync(string queueName)
    {
        throw new NotImplementedException();
    }
    */

    public async Task<SendMessageBatchResultEntry[]> SendMessageBatchAsync(Uri queueUrl, IReadOnlyList<string> messages)
    {
        ArgumentNullException.ThrowIfNull(messages);

        if (messages.Count > 10)
            throw new ArgumentException("Must be 10 or fewer", nameof(messages));

        // Max payload = 256KB (262,144 bytes)

        var parameters = new List<KeyValuePair<string, string>>((messages.Count * 2) + 2) {
            new("Action", "SendMessageBatch")
        };

        for (int i = 0; i < messages.Count; i++)
        {
            int number = i + 1;

            string message = messages[i];
            string prefix = string.Create(CultureInfo.InvariantCulture, $"SendMessageBatchRequestEntry.{number}");

            parameters.Add(new($"{prefix}.Id", i.ToString(CultureInfo.InvariantCulture)));  // Id				Required
            parameters.Add(new($"{prefix}.MessageBody", message));                          // MessageBody		Required
                                                                                            // DelaySeconds	Optional, Max 900(15min)
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
            Content = GetPostContent(parameters)
        };

        string responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return SendMessageBatchResponse.Deserialize(responseText).SendMessageBatchResult.Items;
    }

    public async Task<SendMessageResult> SendMessageAsync(Uri queueUrl, SendMessageRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
            Content = GetPostContent(request.GetParameters())
        };

        var responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        return SendMessageResponse.Deserialize(responseText).SendMessageResult;
    }

    public async Task<SqsMessage[]> ReceiveMessagesAsync(
        Uri queueUrl,
        ReceiveMessagesRequest request, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
            Content = GetPostContent(request.GetParameters())
        };

        string responseText = await SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);

        var response = ReceiveMessageResponse.Deserialize(responseText);

        return response.ReceiveMessageResult.Items ?? Array.Empty<SqsMessage>();
    }

    public async Task<string> DeleteMessageAsync(Uri queueUrl, string recieptHandle)
    {
        ArgumentNullException.ThrowIfNull(recieptHandle);

        var parameters = new List<KeyValuePair<string, string>>(3) {
            new ("Action", "DeleteMessage"),
            new ("ReceiptHandle", recieptHandle)
        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
            Content = GetPostContent(parameters)
        };

        return await SendAsync(httpRequest).ConfigureAwait(false);
    }

    public async Task<string> ChangeMessageVisibilityAsync(Uri queueUrl, ChangeMessageVisibilityRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
            Content = GetPostContent(request.ToParams())
        };

        return await SendAsync(httpRequest).ConfigureAwait(false);
    }

    public async Task<DeleteMessageBatchResultEntry[]> DeleteMessageBatchAsync(Uri queueUrl, string[] recieptHandles)
    {
        ArgumentNullException.ThrowIfNull(recieptHandles);

        if (recieptHandles.Length > 10)
            throw new ArgumentException("Must contain 10 or fewer items", nameof(recieptHandles));

        // Max payload = 64KB (65,536 bytes)

        var parameters = new List<KeyValuePair<string, string>>((recieptHandles.Length * 2) + 2) {
            new("Action", "DeleteMessageBatch")
        };

        for (int i = 0; i < recieptHandles.Length; i++)
        {
            int number    = i + 1;
            string handle = recieptHandles[i];
            string prefix = string.Create(CultureInfo.InvariantCulture, $"DeleteMessageBatchRequestEntry.{number}.");

            parameters.Add(new(prefix + "Id", i.ToString(CultureInfo.InvariantCulture))); // DeleteMessageBatchRequestEntry.n.Id
            parameters.Add(new(prefix + "ReceiptHandle", handle));                        // DeleteMessageBatchRequestEntry.n.ReceiptHandle
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, queueUrl) {
            Content = GetPostContent(parameters)
        };

        string responseText = await SendAsync(httpRequest).ConfigureAwait(false);

        // Because the batch request can result in a combination of successful and unsuccessful actions, 
        // you should check for batch errors even when the call returns an HTTP status code of 200.
        return DeleteMessageBatchResponse.Deserialize(responseText).DeleteMessageBatchResult.Items;
    }

    #region Helpers

    private static FormUrlEncodedContent GetPostContent(List<KeyValuePair<string, string>> parameters)
    {
        parameters.Add(new("Version", Version));

        return new FormUrlEncodedContent(parameters!);
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        if (response.StatusCode is HttpStatusCode.ServiceUnavailable) // 503
        {
            return new ServiceUnavailableException();
        }

        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        try
        {
            var errorResponse = ErrorResponse.Deserialize(responseText);

            return new SqsException(errorResponse.Error);
        }
        catch
        { }

        return new QueueException($"{response.StatusCode}/{responseText}");
    }

    #endregion
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
