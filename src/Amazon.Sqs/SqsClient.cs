using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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

    public Task<GetQueueUrlResult> GetQueueUrlAsync(GetQueueUrlRequest request)
    {
        var requestMessage = ConstructPostRequest("GetQueueUrl", request, SqsSerializerContext.Default.GetQueueUrlRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.GetQueueUrlResult);
    }

    public Task<ListQueuesResult> ListQueuesAsync(ListQueuesRequest request)
    {
        var requestMessage = ConstructPostRequest("ListQueues", request, SqsSerializerContext.Default.ListQueuesRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.ListQueuesResult);
    }

    public async Task SetQueueAttributesAsync(SetQueueAttributesRequest request)
    {
        // empty 200 response on success

        var requestMessage = ConstructPostRequest("SetQueueAttributes", request, SqsSerializerContext.Default.SetQueueAttributesRequest);

        await SendAsync(requestMessage).ConfigureAwait(false);
    }

    public Task<ListDeadLetterSourceQueuesResult> ListDeadLetterSourceQueuesAsync(ListDeadLetterSourceQueuesRequest request)
    {
        var requestMessage = ConstructPostRequest("ListDeadLetterSourceQueues", request, SqsSerializerContext.Default.ListDeadLetterSourceQueuesRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.ListDeadLetterSourceQueuesResult);
    }

    public Task<StartMessageMoveTaskResult> StartMessageMoveTaskAsync(StartMessageMoveTaskRequest request)
    {
        var requestMessage = ConstructPostRequest("StartMessageMoveTask", request, SqsSerializerContext.Default.StartMessageMoveTaskRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.StartMessageMoveTaskResult);
    }

    public Task<CancelMessageMoveTaskResult> CancelMessageMoveTaskAsync(CancelMessageMoveTaskRequest request)
    {
        var requestMessage = ConstructPostRequest("CancelMessageMoveTask", request, SqsSerializerContext.Default.CancelMessageMoveTaskRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.CancelMessageMoveTaskResult);
    }

    public Task<ListMessageMoveTasksResult> ListMessageMoveTasksAsync(ListMessageMoveTasksRequest request)
    {
        var requestMessage = ConstructPostRequest("ListMessageMoveTasks", request, SqsSerializerContext.Default.ListMessageMoveTasksRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.ListMessageMoveTasksResult);
    }

    public async Task AddPermissionAsync(AddPermissionRequest request)
    {
        // empty 200 response on success

        var requestMessage = ConstructPostRequest("AddPermission", request, SqsSerializerContext.Default.AddPermissionRequest);

        await SendAsync(requestMessage).ConfigureAwait(false);
    }

    public async Task RemovePermissionAsync(RemovePermissionRequest request)
    {
        // empty 200 response on success

        var requestMessage = ConstructPostRequest("RemovePermission", request, SqsSerializerContext.Default.RemovePermissionRequest);

        await SendAsync(requestMessage).ConfigureAwait(false);
    }

    public async Task TagQueueAsync(TagQueueRequest request)
    {
        // empty 200 response on success

        var requestMessage = ConstructPostRequest("TagQueue", request, SqsSerializerContext.Default.TagQueueRequest);

        await SendAsync(requestMessage).ConfigureAwait(false);
    }

    public async Task UntagQueueAsync(UntagQueueRequest request)
    {
        // empty 200 response on success

        var requestMessage = ConstructPostRequest("UntagQueue", request, SqsSerializerContext.Default.UntagQueueRequest);

        await SendAsync(requestMessage).ConfigureAwait(false);
    }

    public Task<ListQueueTagsResult> ListQueueTagsAsync(ListQueueTagsRequest request)
    {
        var requestMessage = ConstructPostRequest("ListQueueTags", request, SqsSerializerContext.Default.ListQueueTagsRequest);

        return SendAsync(requestMessage, SqsSerializerContext.Default.ListQueueTagsResult);
    }

    // Stops at the first batch with failures, so that a FIFO queue doesn't receive later messages out of order
    internal async Task SendInBatchesAsync(string queueUrl, IEnumerable<SendMessageBatchRequestEntry> entries)
    {
        foreach (SendMessageBatchRequestEntry[] batch in entries.Chunk(10))
        {
            var result = await SendMessageBatchAsync(new SendMessageBatchRequest(queueUrl, batch)).ConfigureAwait(false);

            if (result.Failed is { Length: > 0 } failed)
            {
                throw new SqsBatchException(failed);
            }
        }
    }

    // https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-making-api-requests-json.html

    private static readonly MediaTypeHeaderValue s_mediaType_xAmzJson1_0 = new("application/x-amz-json-1.0");

    // The docs name the error text both `message` and `Message`
    private static readonly JsonSerializerOptions s_errorJso = new() {
        PropertyNameCaseInsensitive = true,
        TypeInfoResolver = SqsSerializerContext.Default
    };

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
            var errorResult = await response.Content.ReadFromJsonAsync<ErrorResult>(s_errorJso).ConfigureAwait(false);

            return new SqsException(errorResult!, response.StatusCode);
        }
        catch
        {
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            string message = $"status = {response.StatusCode}. response = {responseText}";

            if ((int)response.StatusCode >= 500)
            {
                return new SqsException(new ErrorResult { Message = message }, response.StatusCode);
            }

            return new QueueException(message);
        }
    }

    #endregion
}

// http://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/Welcome.html