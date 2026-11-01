using Amazon.Sqs.Exceptions;

using Carbon.Messaging;

namespace Amazon.Sqs.Tests;

public class SqsQueueTests
{
    private static SqsMessage[] Create(int count)
    {
        return Enumerable.Range(1, count).Select(i => FakeSqs.Received($"h{i}", $"body {i}")).ToArray();
    }

    [Fact]
    public void ConstructorValidatesArguments()
    {
        Assert.ThrowsAny<ArgumentException>(() => new SqsQueue(AwsRegion.USEast1, "", "orders", FakeSqs.Credential));
        Assert.ThrowsAny<ArgumentException>(() => new SqsQueue(AwsRegion.USEast1, null!, "orders", FakeSqs.Credential));
        Assert.ThrowsAny<ArgumentException>(() => new SqsQueue(AwsRegion.USEast1, "123", "", FakeSqs.Credential));
        Assert.ThrowsAny<ArgumentException>(() => new SqsQueue(AwsRegion.USEast1, "123", null!, FakeSqs.Credential));
    }

    [Fact]
    public async Task Get_SendsAShortPollForTheQueue()
    {
        var handler = new FakeSqsHandler(FakeSqs.Messages("hello"));

        var result = await FakeSqs.CreateQueue(handler).GetAsync(5, TimeSpan.FromSeconds(30));

        var message = Assert.Single(result);

        Assert.Equal("id-1", message.Id);
        Assert.Equal("handle-1", message.Receipt.Handle);
        Assert.Equal("hello", message.Body);

        var body = Assert.Single(handler.Requests).Json;

        Assert.Equal(FakeSqs.QueueUrl, body.GetProperty("QueueUrl").GetString());
        Assert.Equal(5,  body.GetProperty("MaxNumberOfMessages").GetInt32());
        Assert.Equal(30, body.GetProperty("VisibilityTimeout").GetInt32());
        Assert.False(body.TryGetProperty("WaitTimeSeconds", out _));
    }

    [Fact]
    public async Task Get_ReturnsEmptyWhenResponseHasNoMessages()
    {
        var result = await FakeSqs.CreateQueue(new FakeSqsHandler(FakeSqs.Empty)).GetAsync(1, null);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Get_DoesNotRetryPermanentErrors()
    {
        var handler = new FakeSqsHandler(FakeSqs.QueueDoesNotExist);

        await Assert.ThrowsAsync<SqsException>(() => FakeSqs.CreateQueue(handler).GetAsync(1, null));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Get_RejectsInvalidTake()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => FakeSqs.CreateQueue(handler).GetAsync(11, null));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Poll_LongPollsUntilMessagesArrive()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty, FakeSqs.Json("""{"Messages":[]}"""), FakeSqs.Messages("hello"));

        var result = await FakeSqs.CreateQueue(handler).PollAsync(2, TimeSpan.FromSeconds(30));

        Assert.Equal("hello", Assert.Single(result).Body);
        Assert.Equal(3, handler.Requests.Count);

        var body = handler.Requests[0].Json;

        Assert.Equal(20, body.GetProperty("WaitTimeSeconds").GetInt32());
        Assert.Equal(2,  body.GetProperty("MaxNumberOfMessages").GetInt32());
        Assert.Equal(30, body.GetProperty("VisibilityTimeout").GetInt32());
    }

    [Fact]
    public async Task Poll_ReturnsEmptyWithoutCallingWhenAlreadyCancelled()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        var result = await FakeSqs.CreateQueue(handler).PollAsync(1, null, new CancellationToken(canceled: true));

        Assert.Empty(result);
        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Poll_StopsWhenCancelledWhileWaiting()
    {
        var handler = new HangingHandler();

        using var cts = new CancellationTokenSource();

        var poll = FakeSqs.CreateQueue(handler).PollAsync(1, null, cts.Token);

        await handler.Started.Task;

        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => poll);
    }

    [Fact]
    public async Task Put_SplitsIntoBatchesOfTen()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"Failed":[],"Successful":[]}"""));

        await FakeSqs.CreateQueue(handler).PutAsync(Create(23));

        Assert.Equal(3, handler.Requests.Count);
        Assert.All(handler.Requests, r => Assert.Equal("AmazonSQS.SendMessageBatch", r.Target));

        var entries = handler.Requests.Select(r => r.Json.GetProperty("Entries").EnumerateArray().ToArray()).ToArray();

        Assert.Equal([10, 10, 3], entries.Select(e => e.Length));

        // Ids only have to be unique within a batch
        Assert.Equal("1", entries[1][0].GetProperty("Id").GetString());

        Assert.Equal(
            Enumerable.Range(1, 23).Select(i => $"body {i}"),
            entries.SelectMany(e => e).Select(e => e.GetProperty("MessageBody").GetString()));
    }

    [Fact]
    public async Task Put_WithNoMessagesSendsNothing()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateQueue(handler).PutAsync();

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Put_SurfacesPartialBatchFailure()
    {
        var handler = new FakeSqsHandler(FakeSqs.BatchWithFailure);

        var ex = await Assert.ThrowsAsync<SqsBatchException>(() => FakeSqs.CreateQueue(handler).PutAsync(Create(2)));

        var failed = Assert.Single(ex.Failed);

        Assert.Equal("2", failed.Id);
        Assert.Equal("InternalError", failed.Code);
        Assert.Contains("InternalError: try again", ex.Message);
    }

    [Fact]
    public async Task Put_StopsAtTheFirstBatchWithFailures()
    {
        var handler = new FakeSqsHandler(FakeSqs.BatchWithFailure);

        await Assert.ThrowsAsync<SqsBatchException>(() => FakeSqs.CreateQueue(handler).PutAsync(Create(11)));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task UpdateMessageVisibility()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateQueue(handler).UpdateMessageVisibilityAsync("handle", TimeSpan.FromMinutes(2));

        var request = Assert.Single(handler.Requests);

        Assert.Equal("AmazonSQS.ChangeMessageVisibility", request.Target);
        Assert.Equal(FakeSqs.QueueUrl, request.Json.GetProperty("QueueUrl").GetString());
        Assert.Equal("handle", request.Json.GetProperty("ReceiptHandle").GetString());
        Assert.Equal(120, request.Json.GetProperty("VisibilityTimeout").GetInt32());
    }

    [Fact]
    public async Task Delete_SendsReceiptHandles()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"Failed":[],"Successful":[]}"""));

        await FakeSqs.CreateQueue(handler).DeleteAsync(Create(2));

        var request = Assert.Single(handler.Requests);

        Assert.Equal("AmazonSQS.DeleteMessageBatch", request.Target);

        var entries = request.Json.GetProperty("Entries");

        Assert.Equal("1",  entries[0].GetProperty("Id").GetString());
        Assert.Equal("h1", entries[0].GetProperty("ReceiptHandle").GetString());
        Assert.Equal("h2", entries[1].GetProperty("ReceiptHandle").GetString());
    }

    [Fact]
    public async Task Delete_DoesNotRetryPermanentErrors()
    {
        var handler = new FakeSqsHandler(FakeSqs.QueueDoesNotExist);

        await Assert.ThrowsAsync<SqsException>(() => FakeSqs.CreateQueue(handler).DeleteAsync(Create(1)));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task Delete_MoreThanTenIsRejected()
    {
        // PutAsync chunks, DeleteAsync does not
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await Assert.ThrowsAsync<ArgumentException>(() => FakeSqs.CreateQueue(handler).DeleteAsync(Create(11)));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Delete_WithNoMessagesSendsNothing()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);
        var queue = FakeSqs.CreateQueue(handler);

        await queue.DeleteAsync();
        await Assert.ThrowsAsync<ArgumentNullException>(() => queue.DeleteAsync(null!));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task Delete_SurfacesPartialBatchFailure()
    {
        var handler = new FakeSqsHandler(FakeSqs.BatchWithFailure);

        var ex = await Assert.ThrowsAsync<SqsBatchException>(() => FakeSqs.CreateQueue(handler).DeleteAsync(Create(2)));

        Assert.Equal("2", Assert.Single(ex.Failed).Id);
        Assert.Single(handler.Requests);
    }
}
