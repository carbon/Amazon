using System.Text.Json;

using Amazon.Sqs.Exceptions;
using Amazon.Sqs.Serialization;

using Carbon.Messaging;

namespace Amazon.Sqs.Tests;

public class FifoAndFairQueueTests
{
    private const string FifoUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/orders.fifo";

    private static readonly Func<HttpResponseMessage> s_sent = FakeSqs.Json("""{"MD5OfMessageBody":"abc","MessageId":"m-1"}""");

    private static readonly Func<HttpResponseMessage> s_batchSent = FakeSqs.Json("""{"Failed":[],"Successful":[]}""");

    private static readonly SqsSendOptions s_fifo = new() {
        MessageGroupId         = "group",
        MessageDeduplicationId = "dedupe"
    };

    private static JsonEncodedMessage<QueueItem>[] Items(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => JsonEncodedMessage<QueueItem>.Create(FakeSqs.Received($"h{i}", $$"""{"Position":{{i}}}""")))
            .ToArray();
    }

    private static SqsMessage[] Strings(int count)
    {
        return Enumerable.Range(1, count).Select(i => FakeSqs.Received($"h{i}", $"body {i}")).ToArray();
    }

    [Fact]
    public void CreateQueue_SerializesFifoAndRedriveAttributes()
    {
        var request = new CreateQueueRequest("orders.fifo", new QueueAttributes {
            FifoQueue                 = "true",
            ContentBasedDeduplication = "true",
            DeduplicationScope        = "messageGroup",
            FifoThroughputLimit       = "perMessageGroupId",
            RedrivePolicy             = "{}",
            RedriveAllowPolicy        = "{}"
        });

        Assert.Equal(
            """{"QueueName":"orders.fifo","Attributes":{"RedrivePolicy":"{}","RedriveAllowPolicy":"{}","FifoQueue":"true","ContentBasedDeduplication":"true","DeduplicationScope":"messageGroup","FifoThroughputLimit":"perMessageGroupId"}}""",
            JsonSerializer.Serialize(request, SqsSerializerContext.Default.CreateQueueRequest));
    }

    [Fact]
    public void ReceiveMessage_SendsSystemAttributeNames()
    {
        var request = new ReceiveMessageRequest("queue-url") {
            MessageSystemAttributeNames = [MessageAttributeNames.MessageGroupId]
        };

        Assert.Equal(
            """{"QueueUrl":"queue-url","MaxNumberOfMessages":1,"MessageSystemAttributeNames":["MessageGroupId"]}""",
            JsonSerializer.Serialize(request, SqsSerializerContext.Default.ReceiveMessageRequest));
    }

    [Fact]
    public async Task ReceivedMessage_ExposesFifoAttributes()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json(
            """{"Messages":[{"Body":"b","Attributes":{"MessageGroupId":"group","MessageDeduplicationId":"dedupe"}}]}"""));

        var result = await FakeSqs.CreateClient(handler).ReceiveMessagesAsync(new ReceiveMessageRequest(FifoUrl));

        var message = Assert.Single(result.Messages!);

        Assert.Equal("group", message.MessageGroupId);
        Assert.Equal("dedupe", message.MessageDeduplicationId);
    }

    [Fact]
    public void Message_WithoutAttributesHasNoGroup()
    {
        var message = new SqsMessage("b");

        Assert.Null(message.MessageGroupId);
        Assert.Null(message.MessageDeduplicationId);
    }

    [Fact]
    public void Identifiers_AreLimitedTo128Characters()
    {
        string limit = new('a', 128);
        string over  = new('a', 129);

        Assert.Equal(limit, new SendMessageRequest("url", "b", messageGroupId: limit).MessageGroupId);
        Assert.Equal(limit, new SendMessageRequest("url", "b", messageDeduplicationId: limit).MessageDeduplicationId);
        Assert.Equal(limit, new SendMessageBatchRequestEntry("1", "b", messageGroupId: limit).MessageGroupId);
        Assert.Equal(limit, new SendMessageBatchRequestEntry("1", "b", messageDeduplicationId: limit).MessageDeduplicationId);

        Assert.Equal("messageGroupId", Assert.Throws<ArgumentOutOfRangeException>(() => new SendMessageRequest("url", "b", messageGroupId: over)).ParamName);
        Assert.Equal("messageDeduplicationId", Assert.Throws<ArgumentOutOfRangeException>(() => new SendMessageRequest("url", "b", messageDeduplicationId: over)).ParamName);
        Assert.Equal("messageGroupId", Assert.Throws<ArgumentOutOfRangeException>(() => new SendMessageBatchRequestEntry("1", "b", messageGroupId: over)).ParamName);
        Assert.Equal("messageDeduplicationId", Assert.Throws<ArgumentOutOfRangeException>(() => new SendMessageBatchRequestEntry("1", "b", messageDeduplicationId: over)).ParamName);
    }

    [Fact]
    public async Task Queue_SendsASingleFifoMessage()
    {
        var handler = new FakeSqsHandler(s_sent);

        var id = await FakeSqs.CreateQueue(handler).PutAsync("hello", s_fifo);

        var body = Assert.Single(handler.Requests).Json;

        Assert.Equal("m-1", id);
        Assert.Equal("hello", body.GetProperty("MessageBody").GetString());
        Assert.Equal("group", body.GetProperty("MessageGroupId").GetString());
        Assert.Equal("dedupe", body.GetProperty("MessageDeduplicationId").GetString());
        Assert.False(body.TryGetProperty("DelaySeconds", out _));
    }

    [Fact]
    public async Task Queue_SendsAFairQueueMessageWithADelay()
    {
        // A standard queue with a tenant as the group
        var handler = new FakeSqsHandler(s_sent);

        await FakeSqs.CreateQueue(handler).PutAsync("hello", new SqsSendOptions { MessageGroupId = "tenant", Delay = TimeSpan.FromSeconds(5) });

        var body = handler.Requests[0].Json;

        Assert.Equal("tenant", body.GetProperty("MessageGroupId").GetString());
        Assert.Equal(5, body.GetProperty("DelaySeconds").GetInt32());
        Assert.False(body.TryGetProperty("MessageDeduplicationId", out _));
    }

    [Fact]
    public async Task Queue_SendsBatchesWithPerMessageOptionsInOrder()
    {
        var handler = new FakeSqsHandler(s_batchSent);

        await FakeSqs.CreateQueue(handler).PutAsync(Strings(12), m => new SqsSendOptions { MessageGroupId = "group-" + m.Body });

        Assert.Equal(2, handler.Requests.Count);

        var entries = handler.Requests.SelectMany(r => r.Json.GetProperty("Entries").EnumerateArray()).ToArray();

        Assert.Equal(12, entries.Length);
        Assert.Equal("body 12", entries[11].GetProperty("MessageBody").GetString());
        Assert.Equal("group-body 12", entries[11].GetProperty("MessageGroupId").GetString());

        // Ids restart in each batch
        Assert.Equal("10", entries[9].GetProperty("Id").GetString());
        Assert.Equal("1", entries[10].GetProperty("Id").GetString());
    }

    [Fact]
    public async Task Queue_BatchesStopAtTheFirstFailure()
    {
        var handler = new FakeSqsHandler(FakeSqs.BatchWithFailure);

        await Assert.ThrowsAsync<SqsBatchException>(() => FakeSqs.CreateQueue(handler).PutAsync(Strings(11), _ => s_fifo));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Queue_OptionsAreValidated()
    {
        var handler = new FakeSqsHandler(s_sent);
        var queue = FakeSqs.CreateQueue(handler);

        await Assert.ThrowsAsync<ArgumentNullException>(() => queue.PutAsync("hello", (SqsSendOptions)null!));
        await Assert.ThrowsAsync<ArgumentNullException>(() => queue.PutAsync((IReadOnlyList<IMessage<string>>)null!, _ => s_fifo));
        await Assert.ThrowsAsync<ArgumentNullException>(() => queue.PutAsync(Strings(1), null!));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Queue_NoMessagesSendsNothing()
    {
        var handler = new FakeSqsHandler(s_batchSent);

        await FakeSqs.CreateQueue(handler).PutAsync(Array.Empty<IMessage<string>>(), _ => s_fifo);

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task TypedQueue_SendsASingleFifoMessage()
    {
        var handler = new FakeSqsHandler(s_sent);

        var id = await FakeSqs.CreateTypedQueue(handler).PutAsync(new QueueItem { Position = 4 }, s_fifo);

        var body = handler.Requests[0].Json;

        Assert.Equal("m-1", id);
        Assert.Equal("""{"Position":4}""", body.GetProperty("MessageBody").GetString());
        Assert.Equal("group", body.GetProperty("MessageGroupId").GetString());
        Assert.Equal("dedupe", body.GetProperty("MessageDeduplicationId").GetString());
    }

    [Fact]
    public async Task TypedQueue_SendsBatchesWithPerMessageOptions()
    {
        var handler = new FakeSqsHandler(s_batchSent);

        await FakeSqs.CreateTypedQueue(handler).PutAsync(Items(11), m => new SqsSendOptions { MessageGroupId = "tenant-" + m.Body.Position });

        Assert.Equal(2, handler.Requests.Count);

        var last = Assert.Single(handler.Requests[1].Json.GetProperty("Entries").EnumerateArray());

        Assert.Equal("""{"Position":11}""", last.GetProperty("MessageBody").GetString());
        Assert.Equal("tenant-11", last.GetProperty("MessageGroupId").GetString());
        Assert.Equal("1", last.GetProperty("Id").GetString());
    }

    [Fact]
    public async Task TypedQueue_OptionsAreValidated()
    {
        var handler = new FakeSqsHandler(s_sent);
        var queue = FakeSqs.CreateTypedQueue(handler);

        await Assert.ThrowsAsync<ArgumentNullException>(() => queue.PutAsync(new QueueItem(), (SqsSendOptions)null!));
        await Assert.ThrowsAsync<ArgumentNullException>(() => queue.PutAsync((IReadOnlyList<IMessage<QueueItem>>)null!, _ => s_fifo));
        await Assert.ThrowsAsync<ArgumentNullException>(() => queue.PutAsync(Items(1), null!));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task BatchResponsesWithoutFailedCountAsSuccess()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        var queue = FakeSqs.CreateQueue(handler);
        var typedQueue = FakeSqs.CreateTypedQueue(handler);

        await queue.PutAsync(Strings(1));
        await queue.PutAsync(Strings(1), _ => s_fifo);
        await queue.DeleteAsync(Strings(1));
        await typedQueue.PutAsync(Items(1));
        await typedQueue.PutAsync(Items(1), _ => s_fifo);
        await typedQueue.DeleteAsync(Items(1));

        Assert.Equal(6, handler.Requests.Count);
    }

    [Fact]
    public async Task StandardQueue_GetSendsNoAttemptId()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateQueue(handler).GetAsync(1, null);

        Assert.False(handler.Requests[0].Json.TryGetProperty("ReceiveRequestAttemptId", out _));
    }

    [Fact]
    public async Task FifoQueue_GetReusesItsAttemptIdWhenRetrying()
    {
        var handler = new FakeSqsHandler(FakeSqs.Unavailable, FakeSqs.Messages("hello"));

        var queue = FakeSqs.UseQueueHandler(new SqsQueue(AwsRegion.USEast1, "123456789012", "orders.fifo", FakeSqs.Credential), handler);

        await queue.GetAsync(1, null);

        Assert.Equal(2, handler.Requests.Count);

        var first = handler.Requests[0].Json;

        Assert.Equal(FifoUrl, first.GetProperty("QueueUrl").GetString());
        Assert.False(string.IsNullOrEmpty(first.GetProperty("ReceiveRequestAttemptId").GetString()));
        Assert.Equal(
            first.GetProperty("ReceiveRequestAttemptId").GetString(),
            handler.Requests[1].Json.GetProperty("ReceiveRequestAttemptId").GetString());
    }
}
