namespace Amazon.Sqs.Tests;

public class SqsClientActionTests
{
    private const string QueueUrl = FakeSqs.QueueUrl;

    private static void AssertSent(FakeSqsHandler handler, string action)
    {
        var request = Assert.Single(handler.Requests);

        Assert.Same(HttpMethod.Post, request.Method);
        Assert.Equal("https://sqs.us-east-1.amazonaws.com/", request.RequestUri.AbsoluteUri);
        Assert.Equal("AmazonSQS." + action, request.Target);
        Assert.Equal("application/x-amz-json-1.0", request.ContentType);
        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", request.Authorization);
        Assert.Contains("/us-east-1/sqs/aws4_request", request.Authorization);
    }

    [Fact]
    public async Task CreateQueue()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json($$"""{"QueueUrl":"{{QueueUrl}}"}"""));

        var result = await FakeSqs.CreateClient(handler).CreateQueueAsync(new CreateQueueRequest("orders"));

        AssertSent(handler, "CreateQueue");
        Assert.Equal(QueueUrl, result.QueueUrl);
        Assert.Equal("orders", handler.Requests[0].Json.GetProperty("QueueName").GetString());
    }

    [Fact]
    public async Task GetQueueAttributes()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"Attributes":{"VisibilityTimeout":"35","DelaySeconds":"45"}}"""));

        var result = await FakeSqs.CreateClient(handler).GetQueueAttributesAsync(
            new GetQueueAttributesRequest(QueueUrl, [QueueAttributeNames.VisibilityTimeout, QueueAttributeNames.DelaySeconds]));

        AssertSent(handler, "GetQueueAttributes");
        Assert.Equal("35", result.Attributes["VisibilityTimeout"]);
        Assert.Equal("45", result.Attributes["DelaySeconds"]);

        var body = handler.Requests[0].Json;

        Assert.Equal(QueueUrl, body.GetProperty("QueueUrl").GetString());
        Assert.Equal(["VisibilityTimeout", "DelaySeconds"], body.GetProperty("AttributeNames").EnumerateArray().Select(e => e.GetString()));
    }

    [Fact]
    public async Task GetQueueAttributes_AllowsResponseWithoutAttributes()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        var result = await FakeSqs.CreateClient(handler).GetQueueAttributesAsync(new GetQueueAttributesRequest(QueueUrl, []));

        Assert.Empty(result.Attributes);
    }

    [Fact]
    public async Task DeleteQueue()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateClient(handler).DeleteQueueAsync(new DeleteQueueRequest(QueueUrl));

        AssertSent(handler, "DeleteQueue");
        Assert.Equal(QueueUrl, handler.Requests[0].Json.GetProperty("QueueUrl").GetString());
    }

    [Fact]
    public async Task PurgeQueue()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateClient(handler).PurgeQueueAsync(new PurgeQueueRequest(QueueUrl));

        AssertSent(handler, "PurgeQueue");
        Assert.Equal(QueueUrl, handler.Requests[0].Json.GetProperty("QueueUrl").GetString());
    }

    [Fact]
    public async Task SendMessage()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json(
            """{"MD5OfMessageBody":"abc","MessageId":"m-1","SequenceNumber":"18849"}"""));

        var result = await FakeSqs.CreateClient(handler).SendMessageAsync(new SendMessageRequest(
            queueUrl               : QueueUrl,
            messageBody            : "hello",
            delay                  : TimeSpan.FromSeconds(30),
            messageDeduplicationId : "dedupe",
            messageGroupId         : "group"));

        AssertSent(handler, "SendMessage");
        Assert.Equal("m-1", result.MessageId);
        Assert.Equal("abc", result.MD5OfMessageBody);
        Assert.Equal("18849", result.SequenceNumber);

        var body = handler.Requests[0].Json;

        Assert.Equal("hello",  body.GetProperty("MessageBody").GetString());
        Assert.Equal(30,       body.GetProperty("DelaySeconds").GetInt32());
        Assert.Equal("dedupe", body.GetProperty("MessageDeduplicationId").GetString());
        Assert.Equal("group",  body.GetProperty("MessageGroupId").GetString());
    }

    [Fact]
    public async Task SendMessageBatch_ReportsPartialFailure()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json(
            """
            {
              "Failed": [ { "Id": "2", "Code": "InvalidMessageContents", "SenderFault": true, "Message": "bad" } ],
              "Successful": [ { "Id": "1", "MD5OfMessageBody": "abc", "MessageId": "m-1" } ]
            }
            """));

        var result = await FakeSqs.CreateClient(handler).SendMessageBatchAsync(new SendMessageBatchRequest(QueueUrl, ["a", "b"]));

        AssertSent(handler, "SendMessageBatch");
        Assert.Equal("m-1", Assert.Single(result.Successful).MessageId);

        var failed = Assert.Single(result.Failed);

        Assert.Equal("2", failed.Id);
        Assert.Equal("InvalidMessageContents", failed.Code);
        Assert.True(failed.SenderFault);
        Assert.Equal("bad", failed.Message);
    }

    [Fact]
    public async Task ReceiveMessages()
    {
        var handler = new FakeSqsHandler(FakeSqs.Messages("first", "second"));

        var result = await FakeSqs.CreateClient(handler).ReceiveMessagesAsync(new ReceiveMessageRequest(QueueUrl, maxNumberOfMessages: 2));

        AssertSent(handler, "ReceiveMessage");
        Assert.Equal(["first", "second"], result.Messages!.Select(m => m.Body));
        Assert.Equal(2, handler.Requests[0].Json.GetProperty("MaxNumberOfMessages").GetInt32());
    }

    [Fact]
    public async Task ReceiveMessages_HonoursCancellationWhileWaiting()
    {
        var handler = new HangingHandler();

        using var cts = new CancellationTokenSource();

        var receive = FakeSqs.CreateClient(handler).ReceiveMessagesAsync(new ReceiveMessageRequest(QueueUrl), cts.Token);

        await handler.Started.Task;

        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => receive);
    }

    [Theory]
    [InlineData("String.custom", MessageAttributeDataType.String)]
    [InlineData("Number.int", MessageAttributeDataType.Number)]
    [InlineData("Binary.png", MessageAttributeDataType.Binary)]
    public async Task ReceiveMessages_AcceptsCustomDataTypeLabels(string dataType, MessageAttributeDataType expected)
    {
        var handler = new FakeSqsHandler(FakeSqs.Json(
            $$"""
            {
              "Messages": [ {
                "MessageId": "m-1",
                "ReceiptHandle": "h-1",
                "Body": "hello",
                "MessageAttributes": { "a": { "DataType": "{{dataType}}", "StringValue": "1" } }
              } ]
            }
            """));

        var result = await FakeSqs.CreateClient(handler).ReceiveMessagesAsync(new ReceiveMessageRequest(QueueUrl));

        var message = Assert.Single(result.Messages!);

        Assert.Equal("hello", message.Body);
        Assert.Equal(expected, message.MessageAttributes["a"].DataType);
        Assert.Equal("1", message.MessageAttributes["a"].StringValue);
    }

    [Theory]
    [InlineData("Custom")]
    [InlineData("string")]
    [InlineData("")]
    public async Task ReceiveMessages_RejectsUnknownDataTypes(string dataType)
    {
        var handler = new FakeSqsHandler(FakeSqs.Json(
            $$"""
            {
              "Messages": [ {
                "Body": "b",
                "MessageAttributes": { "a": { "DataType": "{{dataType}}" } }
              } ]
            }
            """));

        await Assert.ThrowsAsync<System.Text.Json.JsonException>(() => FakeSqs.CreateClient(handler).ReceiveMessagesAsync(new ReceiveMessageRequest(QueueUrl)));
    }

    [Fact]
    public async Task DeleteMessage()
    {
        var handler = new FakeSqsHandler(FakeSqs.Content("", "application/x-amz-json-1.0", System.Net.HttpStatusCode.OK));

        await FakeSqs.CreateClient(handler).DeleteMessageAsync(new DeleteMessageRequest(QueueUrl, "handle"));

        AssertSent(handler, "DeleteMessage");
        Assert.Equal("handle", handler.Requests[0].Json.GetProperty("ReceiptHandle").GetString());
    }

    [Fact]
    public async Task DeleteMessageBatch()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"Failed":[],"Successful":[{"Id":"1"},{"Id":"2"}]}"""));

        string[] handles = ["h1", "h2"];

        var result = await FakeSqs.CreateClient(handler).DeleteMessageBatchAsync(new DeleteMessageBatchRequest(QueueUrl, handles));

        AssertSent(handler, "DeleteMessageBatch");
        Assert.Equal(["1", "2"], result.Successful.Select(e => e.Id));
        Assert.Empty(result.Failed);
    }

    [Fact]
    public async Task ChangeMessageVisibility()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateClient(handler).ChangeMessageVisibilityAsync(new ChangeMessageVisibilityRequest(QueueUrl, "handle", 45));

        AssertSent(handler, "ChangeMessageVisibility");

        var body = handler.Requests[0].Json;

        Assert.Equal("handle", body.GetProperty("ReceiptHandle").GetString());
        Assert.Equal(45,       body.GetProperty("VisibilityTimeout").GetInt32());
    }

    [Fact]
    public async Task ChangeMessageVisibilityBatch()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json(
            """{"Failed":[{"Id":"b","Code":"ReceiptHandleIsInvalid","SenderFault":true,"Message":"bad handle"}],"Successful":[{"Id":"a"}]}"""));

        var result = await FakeSqs.CreateClient(handler).ChangeMessageVisibilityBatchAsync(new ChangeMessageVisibilityBatchRequest(QueueUrl, [
            new ChangeMessageVisibilityBatchRequestEntry("a", "h-a", 45),
            new ChangeMessageVisibilityBatchRequestEntry("b", "h-b", 60)
        ]));

        AssertSent(handler, "ChangeMessageVisibilityBatch");
        Assert.Equal("a", Assert.Single(result.Successful).Id);

        var failed = Assert.Single(result.Failed);

        Assert.Equal("b", failed.Id);
        Assert.Equal("ReceiptHandleIsInvalid", failed.Code);
        Assert.True(failed.SenderFault);
        Assert.Equal("bad handle", failed.Message);

        var entries = handler.Requests[0].Json.GetProperty("Entries");

        Assert.Equal("h-b", entries[1].GetProperty("ReceiptHandle").GetString());
        Assert.Equal(60,    entries[1].GetProperty("VisibilityTimeout").GetInt32());
    }
}
