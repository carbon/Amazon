namespace Amazon.Sqs.Tests;

public class SqsNewActionTests
{
    private const string QueueUrl = FakeSqs.QueueUrl;

    private const string Arn = "arn:aws:sqs:us-east-1:123456789012:orders-dlq";

    private static RecordedRequest AssertSent(FakeSqsHandler handler, string action)
    {
        var request = Assert.Single(handler.Requests);

        Assert.Equal("AmazonSQS." + action, request.Target);
        Assert.Equal("application/x-amz-json-1.0", request.ContentType);

        return request;
    }

    private static string[] Strings(System.Text.Json.JsonElement array)
    {
        return array.EnumerateArray().Select(e => e.GetString()!).ToArray();
    }

    [Fact]
    public async Task GetQueueUrl()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json($$"""{"QueueUrl":"{{QueueUrl}}"}"""));

        var result = await FakeSqs.CreateClient(handler).GetQueueUrlAsync(new GetQueueUrlRequest("orders", "111122223333"));

        var body = AssertSent(handler, "GetQueueUrl").Json;

        Assert.Equal(QueueUrl, result.QueueUrl);
        Assert.Equal("orders", body.GetProperty("QueueName").GetString());
        Assert.Equal("111122223333", body.GetProperty("QueueOwnerAWSAccountId").GetString());
    }

    [Fact]
    public async Task GetQueueUrl_OmitsTheOwnerByDefault()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json($$"""{"QueueUrl":"{{QueueUrl}}"}"""));

        await FakeSqs.CreateClient(handler).GetQueueUrlAsync(new GetQueueUrlRequest("orders"));

        Assert.Equal("""{"QueueName":"orders"}""", handler.Requests[0].Body);
    }

    [Fact]
    public async Task ListQueues_ReturnsAPageAndItsToken()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json($$"""{"NextToken":"next","QueueUrls":["{{QueueUrl}}"]}"""));

        var result = await FakeSqs.CreateClient(handler).ListQueuesAsync(new ListQueuesRequest("ord", 5, "previous"));

        var body = AssertSent(handler, "ListQueues").Json;

        Assert.Equal("next", result.NextToken);
        Assert.Equal(QueueUrl, Assert.Single(result.QueueUrls));
        Assert.Equal("ord", body.GetProperty("QueueNamePrefix").GetString());
        Assert.Equal(5, body.GetProperty("MaxResults").GetInt32());
        Assert.Equal("previous", body.GetProperty("NextToken").GetString());
    }

    [Fact]
    public async Task ListQueues_AllowsAnEmptyResponse()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        var result = await FakeSqs.CreateClient(handler).ListQueuesAsync(new ListQueuesRequest());

        Assert.Equal("{}", handler.Requests[0].Body);
        Assert.Empty(result.QueueUrls);
        Assert.Null(result.NextToken);
    }

    [Fact]
    public async Task SetQueueAttributes()
    {
        var handler = new FakeSqsHandler(FakeSqs.Content("", "application/x-amz-json-1.0", System.Net.HttpStatusCode.OK));

        await FakeSqs.CreateClient(handler).SetQueueAttributesAsync(new SetQueueAttributesRequest(QueueUrl, new QueueAttributes {
            VisibilityTimeout = 35,
            RedrivePolicy     = "{}"
        }));

        var body = AssertSent(handler, "SetQueueAttributes").Json;

        Assert.Equal(QueueUrl, body.GetProperty("QueueUrl").GetString());
        Assert.Equal("35", body.GetProperty("Attributes").GetProperty("VisibilityTimeout").GetString());
        Assert.Equal("{}", body.GetProperty("Attributes").GetProperty("RedrivePolicy").GetString());
    }

    [Fact]
    public async Task ListDeadLetterSourceQueues_ReadsLowerCamelCaseUrls()
    {
        // The field is documented as queueUrls here, and QueueUrls on ListQueues
        var handler = new FakeSqsHandler(FakeSqs.Json($$"""{"NextToken":"next","queueUrls":["{{QueueUrl}}"]}"""));

        var result = await FakeSqs.CreateClient(handler).ListDeadLetterSourceQueuesAsync(new ListDeadLetterSourceQueuesRequest(QueueUrl, 2, "previous"));

        var body = AssertSent(handler, "ListDeadLetterSourceQueues").Json;

        Assert.Equal(QueueUrl, Assert.Single(result.QueueUrls));
        Assert.Equal("next", result.NextToken);
        Assert.Equal(2, body.GetProperty("MaxResults").GetInt32());
        Assert.Equal("previous", body.GetProperty("NextToken").GetString());
    }

    [Fact]
    public async Task ListDeadLetterSourceQueues_AllowsAnEmptyResponse()
    {
        var result = await FakeSqs.CreateClient(new FakeSqsHandler(FakeSqs.Empty)).ListDeadLetterSourceQueuesAsync(new ListDeadLetterSourceQueuesRequest(QueueUrl));

        Assert.Empty(result.QueueUrls);
    }

    [Fact]
    public async Task StartMessageMoveTask()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"TaskHandle":"task"}"""));

        var result = await FakeSqs.CreateClient(handler).StartMessageMoveTaskAsync(new StartMessageMoveTaskRequest(Arn, "arn:destination", 10));

        var body = AssertSent(handler, "StartMessageMoveTask").Json;

        Assert.Equal("task", result.TaskHandle);
        Assert.Equal(Arn, body.GetProperty("SourceArn").GetString());
        Assert.Equal("arn:destination", body.GetProperty("DestinationArn").GetString());
        Assert.Equal(10, body.GetProperty("MaxNumberOfMessagesPerSecond").GetInt32());
    }

    [Fact]
    public async Task CancelMessageMoveTask()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"ApproximateNumberOfMessagesMoved":300}"""));

        var result = await FakeSqs.CreateClient(handler).CancelMessageMoveTaskAsync(new CancelMessageMoveTaskRequest("task"));

        Assert.Equal("task", AssertSent(handler, "CancelMessageMoveTask").Json.GetProperty("TaskHandle").GetString());
        Assert.Equal(300, result.ApproximateNumberOfMessagesMoved);
    }

    [Fact]
    public async Task ListMessageMoveTasks()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json(
            """
            {
              "Results": [
                {
                  "ApproximateNumberOfMessagesMoved": 50,
                  "ApproximateNumberOfMessagesToMove": 100,
                  "DestinationArn": "arn:destination",
                  "MaxNumberOfMessagesPerSecond": 10,
                  "SourceArn": "arn:source",
                  "StartedTimestamp": 1684429993999,
                  "Status": "RUNNING",
                  "TaskHandle": "task"
                },
                {
                  "ApproximateNumberOfMessagesMoved": 1,
                  "FailureReason": "AWS.SimpleQueueService.KmsAccessDenied",
                  "StartedTimestamp": 1684429053010,
                  "Status": "FAILED"
                }
              ]
            }
            """));

        var result = await FakeSqs.CreateClient(handler).ListMessageMoveTasksAsync(new ListMessageMoveTasksRequest(Arn, 5));

        var body = AssertSent(handler, "ListMessageMoveTasks").Json;

        Assert.Equal(5, body.GetProperty("MaxResults").GetInt32());
        Assert.Equal(2, result.Results.Length);

        var running = result.Results[0];

        Assert.Equal(50, running.ApproximateNumberOfMessagesMoved);
        Assert.Equal(100, running.ApproximateNumberOfMessagesToMove);
        Assert.Equal("arn:destination", running.DestinationArn);
        Assert.Equal(10, running.MaxNumberOfMessagesPerSecond);
        Assert.Equal("arn:source", running.SourceArn);
        Assert.Equal(1684429993999, running.StartedTimestamp);
        Assert.Equal("RUNNING", running.Status);
        Assert.Equal("task", running.TaskHandle);
        Assert.Null(running.FailureReason);

        Assert.Equal("AWS.SimpleQueueService.KmsAccessDenied", result.Results[1].FailureReason);
        Assert.Null(result.Results[1].TaskHandle);
    }

    [Fact]
    public async Task ListMessageMoveTasks_AllowsAnEmptyResponse()
    {
        var result = await FakeSqs.CreateClient(new FakeSqsHandler(FakeSqs.Empty)).ListMessageMoveTasksAsync(new ListMessageMoveTasksRequest(Arn));

        Assert.Empty(result.Results);
    }

    [Fact]
    public async Task AddPermission()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateClient(handler).AddPermissionAsync(new AddPermissionRequest(QueueUrl, "MyLabel", ["177715257436"], ["SendMessage", "ReceiveMessage"]));

        var body = AssertSent(handler, "AddPermission").Json;

        Assert.Equal("MyLabel", body.GetProperty("Label").GetString());
        Assert.Equal(["177715257436"], Strings(body.GetProperty("AWSAccountIds")));
        Assert.Equal(["SendMessage", "ReceiveMessage"], Strings(body.GetProperty("Actions")));
    }

    [Fact]
    public async Task RemovePermission()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateClient(handler).RemovePermissionAsync(new RemovePermissionRequest(QueueUrl, "MyLabel"));

        Assert.Equal("MyLabel", AssertSent(handler, "RemovePermission").Json.GetProperty("Label").GetString());
    }

    [Fact]
    public async Task TagQueue()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateClient(handler).TagQueueAsync(new TagQueueRequest(QueueUrl, new() { ["QueueType"] = "Production" }));

        // Unlike the lower case tags of CreateQueue
        Assert.Equal("Production", AssertSent(handler, "TagQueue").Json.GetProperty("Tags").GetProperty("QueueType").GetString());
    }

    [Fact]
    public async Task UntagQueue()
    {
        var handler = new FakeSqsHandler(FakeSqs.Empty);

        await FakeSqs.CreateClient(handler).UntagQueueAsync(new UntagQueueRequest(QueueUrl, ["QueueType"]));

        Assert.Equal(["QueueType"], Strings(AssertSent(handler, "UntagQueue").Json.GetProperty("TagKeys")));
    }

    [Fact]
    public async Task ListQueueTags()
    {
        var handler = new FakeSqsHandler(FakeSqs.Json("""{"Tags":{"QueueType":"Production"}}"""));

        var result = await FakeSqs.CreateClient(handler).ListQueueTagsAsync(new ListQueueTagsRequest(QueueUrl));

        Assert.Equal(QueueUrl, AssertSent(handler, "ListQueueTags").Json.GetProperty("QueueUrl").GetString());
        Assert.Equal("Production", result.Tags["QueueType"]);
    }

    [Fact]
    public async Task ListQueueTags_AllowsAQueueWithoutTags()
    {
        var result = await FakeSqs.CreateClient(new FakeSqsHandler(FakeSqs.Empty)).ListQueueTagsAsync(new ListQueueTagsRequest(QueueUrl));

        Assert.Empty(result.Tags);
    }

    [Fact]
    public void Requests_ValidateArguments()
    {
        Assert.ThrowsAny<ArgumentException>(() => new GetQueueUrlRequest(""));
        Assert.ThrowsAny<ArgumentException>(() => new SetQueueAttributesRequest("", new QueueAttributes()));
        Assert.Throws<ArgumentNullException>(() => new SetQueueAttributesRequest(QueueUrl, null!));
        Assert.ThrowsAny<ArgumentException>(() => new ListDeadLetterSourceQueuesRequest(""));
        Assert.ThrowsAny<ArgumentException>(() => new StartMessageMoveTaskRequest(""));
        Assert.ThrowsAny<ArgumentException>(() => new CancelMessageMoveTaskRequest(""));
        Assert.ThrowsAny<ArgumentException>(() => new ListMessageMoveTasksRequest(""));
        Assert.ThrowsAny<ArgumentException>(() => new RemovePermissionRequest("", "label"));
        Assert.ThrowsAny<ArgumentException>(() => new RemovePermissionRequest(QueueUrl, ""));
        Assert.ThrowsAny<ArgumentException>(() => new TagQueueRequest("", []));
        Assert.Throws<ArgumentNullException>(() => new TagQueueRequest(QueueUrl, null!));
        Assert.ThrowsAny<ArgumentException>(() => new UntagQueueRequest("", []));
        Assert.Throws<ArgumentNullException>(() => new UntagQueueRequest(QueueUrl, null!));
        Assert.ThrowsAny<ArgumentException>(() => new ListQueueTagsRequest(""));
    }

    [Fact]
    public void AddPermission_ValidatesArguments()
    {
        Assert.ThrowsAny<ArgumentException>(() => new AddPermissionRequest("", "label", [], []));
        Assert.ThrowsAny<ArgumentException>(() => new AddPermissionRequest(QueueUrl, "", [], []));
        Assert.Throws<ArgumentNullException>(() => new AddPermissionRequest(QueueUrl, "label", null!, []));
        Assert.Throws<ArgumentNullException>(() => new AddPermissionRequest(QueueUrl, "label", [], null!));

        Assert.Throws<ArgumentOutOfRangeException>(() => new AddPermissionRequest(QueueUrl, new string('a', 81), [], []));
        Assert.Equal(80, new AddPermissionRequest(QueueUrl, new string('a', 80), [], []).Label.Length);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1001)]
    public void PageSizes_MustBeInRange(int maxResults)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ListQueuesRequest(maxResults: maxResults));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ListDeadLetterSourceQueuesRequest(QueueUrl, maxResults));
    }

    [Fact]
    public void Limits_AcceptTheirBoundaries()
    {
        Assert.Equal(1000, new ListQueuesRequest(maxResults: 1000).MaxResults);
        Assert.Equal(1000, new ListDeadLetterSourceQueuesRequest(QueueUrl, 1000).MaxResults);
        Assert.Equal(10, new ListMessageMoveTasksRequest(Arn, 10).MaxResults);
        Assert.Equal(500, new StartMessageMoveTaskRequest(Arn, maxNumberOfMessagesPerSecond: 500).MaxNumberOfMessagesPerSecond);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void ListMessageMoveTasks_MaxResultsMustBeInRange(int maxResults)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ListMessageMoveTasksRequest(Arn, maxResults));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void StartMessageMoveTask_RateMustBeInRange(int rate)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new StartMessageMoveTaskRequest(Arn, maxNumberOfMessagesPerSecond: rate));
    }
}
