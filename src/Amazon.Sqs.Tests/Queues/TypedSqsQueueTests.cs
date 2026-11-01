using Amazon.Exceptions;
using Amazon.Sqs.Exceptions;

using System.Text.Json;

namespace Amazon.Sqs.Tests;

public class TypedSqsQueueTests
{
    private static JsonEncodedMessage<QueueItem>[] Create(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => JsonEncodedMessage<QueueItem>.Create(FakeSqs.Received($"h{i}", $$"""{"Position":{{i}}}""")))
            .ToArray();
    }

    [Fact]
    public void ConstructorValidatesArguments()
    {
        Assert.ThrowsAny<ArgumentException>(() => new SqsQueue<QueueItem>(AwsRegion.USEast1, "", "orders", FakeSqs.Credential));
        Assert.ThrowsAny<ArgumentException>(() => new SqsQueue<QueueItem>(AwsRegion.USEast1, "123", "", FakeSqs.Credential));
    }

    [Fact]
    public async Task Get_DecodesMessages()
    {
        var handler = new FakeSqsHandler(FakeSqs.Messages("""{"Position":7,"Text":"hello"}"""));

        var result = await FakeSqs.CreateTypedQueue(handler).GetAsync(3, TimeSpan.FromSeconds(30));

        var message = Assert.Single(result);

        Assert.Equal("id-1", message.Id);
        Assert.Equal("handle-1", message.Receipt.Handle);
        Assert.Equal(7, message.Body.Position);
        Assert.Equal("hello", message.Body.Text);

        var body = Assert.Single(handler.Requests).Json;

        Assert.Equal(FakeSqs.QueueUrl, body.GetProperty("QueueUrl").GetString());
        Assert.Equal(3, body.GetProperty("MaxNumberOfMessages").GetInt32());
        Assert.False(body.TryGetProperty("WaitTimeSeconds", out _));
    }

    [Fact]
    public async Task Get_ReturnsEmptyWhenResponseHasNoMessages()
    {
        Assert.Empty(await FakeSqs.CreateTypedQueue(new FakeSqsHandler(FakeSqs.Empty)).GetAsync(1, null));
    }

    [Fact]
    public async Task Get_MalformedBodyFailsTheWholeBatch()
    {
        // The good message in the batch is lost to the caller too, and stays invisible until its timeout
        var handler = new FakeSqsHandler(FakeSqs.Messages("""{"Position":1}""", "not json"));

        await Assert.ThrowsAsync<JsonException>(() => FakeSqs.CreateTypedQueue(handler).GetAsync(2, null));
    }

    [Fact]
    public async Task Get_DoesNotRetry()
    {
        // Unlike SqsQueue.GetAsync
        var handler = new FakeSqsHandler(FakeSqs.Unavailable);

        await Assert.ThrowsAsync<ServiceUnavailableException>(() => FakeSqs.CreateTypedQueue(handler).GetAsync(1, null));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Poll_LongPollsUntilMessagesArrive()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty, FakeSqs.Messages("""{"Position":2}"""));

        var result = await FakeSqs.CreateTypedQueue(handler).PollAsync(1, TimeSpan.FromSeconds(30));

        Assert.Equal(2, Assert.Single(result).Body.Position);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(20, handler.Requests[0].Json.GetProperty("WaitTimeSeconds").GetInt32());
    }

    [Fact]
    public async Task Poll_ReturnsEmptyWithoutCallingWhenAlreadyCancelled()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        Assert.Empty(await FakeSqs.CreateTypedQueue(handler).PollAsync(1, null, new CancellationToken(canceled: true)));
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Put_SingleMessageIsEncodedAndReturnsItsId()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"MD5OfMessageBody":"abc","MessageId":"m-1"}"""));

        var id = await FakeSqs.CreateTypedQueue(handler).PutAsync(new QueueItem { Position = 4 }, TimeSpan.FromSeconds(10));

        Assert.Equal("m-1", id);

        var request = Assert.Single(handler.Requests);

        Assert.Equal("AmazonSQS.SendMessage", request.Target);
        Assert.Equal(FakeSqs.QueueUrl, request.Json.GetProperty("QueueUrl").GetString());
        Assert.Equal(10, request.Json.GetProperty("DelaySeconds").GetInt32());

        // Null properties are left out
        Assert.Equal("""{"Position":4}""", request.Json.GetProperty("MessageBody").GetString());
    }

    [Fact]
    public async Task Put_SplitsIntoBatchesOfTen()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"Failed":[],"Successful":[]}"""));

        await FakeSqs.CreateTypedQueue(handler).PutAsync(Create(11));

        Assert.Equal(2, handler.Requests.Count);

        var last = Assert.Single(handler.Requests[1].Json.GetProperty("Entries").EnumerateArray());

        Assert.Equal("""{"Position":11}""", last.GetProperty("MessageBody").GetString());
    }

    [Fact]
    public async Task Put_ValidatesAndIgnoresEmptyInput()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);
        var queue = FakeSqs.CreateTypedQueue(handler);

        await Assert.ThrowsAsync<ArgumentNullException>(() => queue.PutAsync((Carbon.Messaging.IMessage<QueueItem>[])null!));
        await queue.PutAsync(Array.Empty<Carbon.Messaging.IMessage<QueueItem>>());

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Put_SurfacesPartialBatchFailure()
    {
        var handler = new FakeSqsHandler(FakeSqs.BatchWithFailure);

        var ex = await Assert.ThrowsAsync<SqsBatchException>(() => FakeSqs.CreateTypedQueue(handler).PutAsync(Create(2)));

        Assert.Equal("InternalError", Assert.Single(ex.Failed).Code);
    }

    [Fact]
    public async Task UpdateMessageVisibility()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateTypedQueue(handler).UpdateMessageVisibilityAsync("handle", TimeSpan.FromSeconds(90));

        var request = Assert.Single(handler.Requests);

        Assert.Equal("AmazonSQS.ChangeMessageVisibility", request.Target);
        Assert.Equal("handle", request.Json.GetProperty("ReceiptHandle").GetString());
        Assert.Equal(90, request.Json.GetProperty("VisibilityTimeout").GetInt32());
    }

    [Fact]
    public async Task Delete_SendsReceiptHandles()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"Failed":[],"Successful":[]}"""));

        await FakeSqs.CreateTypedQueue(handler).DeleteAsync(Create(2));

        var entries = Assert.Single(handler.Requests).Json.GetProperty("Entries");

        Assert.Equal("h1", entries[0].GetProperty("ReceiptHandle").GetString());
        Assert.Equal("h2", entries[1].GetProperty("ReceiptHandle").GetString());
    }

    [Fact]
    public async Task Delete_SplitsIntoBatchesOfTen()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"Failed":[],"Successful":[]}"""));

        await FakeSqs.CreateTypedQueue(handler).DeleteAsync(Create(11));

        Assert.Equal(2, handler.Requests.Count);

        var last = Assert.Single(handler.Requests[1].Json.GetProperty("Entries").EnumerateArray());

        Assert.Equal("h11", last.GetProperty("ReceiptHandle").GetString());
    }

    [Fact]
    public async Task Delete_StopsAtTheFirstBatchWithFailures()
    {
        var handler = new FakeSqsHandler(FakeSqs.BatchWithFailure);

        await Assert.ThrowsAsync<SqsBatchException>(() => FakeSqs.CreateTypedQueue(handler).DeleteAsync(Create(11)));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Delete_ValidatesAndIgnoresEmptyInput()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);
        var queue = FakeSqs.CreateTypedQueue(handler);

        await Assert.ThrowsAsync<ArgumentNullException>(() => queue.DeleteAsync(null!));
        await queue.DeleteAsync();

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Delete_DoesNotRetryPermanentErrors()
    {
        var handler = new FakeSqsHandler(FakeSqs.QueueDoesNotExist);

        await Assert.ThrowsAsync<SqsException>(() => FakeSqs.CreateTypedQueue(handler).DeleteAsync(Create(1)));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Delete_RetriesTransientErrorsThenGivesUpAfterFourAttempts()
    {
        var handler = new FakeSqsHandler(FakeSqs.Unavailable);

        await Assert.ThrowsAsync<ServiceUnavailableException>(() => FakeSqs.CreateTypedQueue(handler).DeleteAsync(Create(1)));

        Assert.Equal(4, handler.Requests.Count);
    }

    [Fact]
    public async Task Delete_SurfacesPartialBatchFailure()
    {
        var handler = new FakeSqsHandler(FakeSqs.BatchWithFailure);

        var ex = await Assert.ThrowsAsync<SqsBatchException>(() => FakeSqs.CreateTypedQueue(handler).DeleteAsync(Create(2)));

        Assert.Equal("2", Assert.Single(ex.Failed).Id);
        Assert.Single(handler.Requests);
    }
}
