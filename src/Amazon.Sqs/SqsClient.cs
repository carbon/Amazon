using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;

using Amazon.Exceptions;
using Amazon.Sqs.Exceptions;
using Amazon.Sqs.Serialization;

using Carbon.Messaging;

namespace Amazon.Sqs;

public sealed class SqsClient(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Sqs, region, credential)
{
    public Task<CreateQueueResult> CreateQueueAsync(CreateQueueRequest request)
    {
        var requestMessage = ConstructPostRequest("CreateQueue", request, SqsSerializerContext.Default.CreateQueueRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.CreateQueueResult);
    }

    public Task<GetQueueAttributesResult> GetQueueAttributesAsync(GetQueueAttributesRequest request)
    {
        var requestMessage = ConstructPostRequest("GetQueueAttributes", request, SqsSerializerContext.Default.GetQueueAttributesRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.GetQueueAttributesResult);
    }

    public async Task DeleteQueueAsync(DeleteQueueRequest request)
    {
        var requestMessage = ConstructPostRequest("DeleteQueue", request, SqsSerializerContext.Default.DeleteQueueRequest);

        await SendAsync(requestMessage).ConfigureAwait(false);
    }


    public async Task PurgeQueueAsync(PurgeQueueRequest request)
    {
        var requestMessage = ConstructPostRequest("PurgeQueue", request, SqsSerializerContext.Default.PurgeQueueRequest);

        await SendAsync(requestMessage).ConfigureAwait(false);
    }

    public Task<SendMessageBatchResult> SendMessageBatchAsync(SendMessageBatchRequest request)
    {
        var requestMessage = ConstructPostRequest("SendMessageBatch", request, SqsSerializerContext.Default.SendMessageBatchRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.SendMessageBatchResult);
    }

    public Task<SendMessageResult> SendMessageAsync(SendMessageRequest request)
    {
        var requestMessage = ConstructPostRequest("SendMessage", request, SqsSerializerContext.Default.SendMessageRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.SendMessageResult);
    }

    public Task<ReceiveMessageResult> ReceiveMessagesAsync(ReceiveMessageRequest request, CancellationToken cancellationToken = default)
    {
        var requestMessage = ConstructPostRequest("ReceiveMessage", request, SqsSerializerContext.Default.ReceiveMessageRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.ReceiveMessageResult, cancellationToken);
    }

    public async Task DeleteMessageAsync(DeleteMessageRequest request)
    {
        // empty 200 response on success

        var requestMessage = ConstructPostRequest("DeleteMessage", request, SqsSerializerContext.Default.DeleteMessageRequest);

        await SendAsync(requestMessage).ConfigureAwait(false);
    }

    public async Task ChangeMessageVisibilityAsync(ChangeMessageVisibilityRequest request)
    {
        // empty 200 response on success

        var requestMessage = ConstructPostRequest("ChangeMessageVisibility", request, SqsSerializerContext.Default.ChangeMessageVisibilityRequest);

        await SendAsync(requestMessage).ConfigureAwait(false);
    }

    public async Task<ChangeMessageVisibilityBatchResult> ChangeMessageVisibilityBatchAsync(ChangeMessageVisibilityBatchRequest request)
    {
        var requestMessage = ConstructPostRequest("ChangeMessageVisibilityBatch", request, SqsSerializerContext.Default.ChangeMessageVisibilityBatchRequest);

        return await SendAsync(requestMessage, SqsSerializerContext.Default.ChangeMessageVisibilityBatchResult).ConfigureAwait(false);
    }

    public Task<DeleteMessageBatchResult> DeleteMessageBatchAsync(DeleteMessageBatchRequest request)
    {
        var requestMessage = ConstructPostRequest("DeleteMessageBatch", request, SqsSerializerContext.Default.DeleteMessageBatchRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.DeleteMessageBatchResult);
    }

    // https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-making-api-requests-json.html

    private static readonly MediaTypeHeaderValue s_mediaType_xAmzJson1_0 = new("application/x-amz-json-1.0");

    internal HttpRequestMessage ConstructPostRequest<TRequest>(
        [ConstantExpected] string action,
        TRequest request,
        JsonTypeInfo<TRequest> jsonTypeInfo)
        where TRequest : SqsRequest
    {
        return new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Headers = {
                { "X-Amz-Target", $"AmazonSQS.{action}" },
            },
            Content = JsonContent.Create(request, jsonTypeInfo, mediaType: s_mediaType_xAmzJson1_0)
        };
    }

    private async Task<TResult> SendAsync<TResult>(
        HttpRequestMessage request,
        JsonTypeInfo<TResult> jsonTypeInfo,
        CancellationToken cancellationToken = default)
    {
        await SignAsync(request).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await GetExceptionAsync(response).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync(jsonTypeInfo, cancellationToken).ConfigureAwait(false);

        return result!;
    }

    #region Helpers

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        if (response.StatusCode is HttpStatusCode.ServiceUnavailable) // 503
        {
            return new ServiceUnavailableException();
        }

        try
        {
            var errorResult = await response.Content.ReadFromJsonAsync<ErrorResult>().ConfigureAwait(false);

            return new SqsException(errorResult!);
        }
        catch
        {
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new QueueException($"status = {response.StatusCode}. response = {responseText}");
        }
    }

    #endregion
}

// http://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/Welcome.html