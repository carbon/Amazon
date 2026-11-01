using System.Globalization;
using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Tests;

public class RequestValidationTests
{
    private const string Url = "queue-url";

    [Fact]
    public void ReceiveMessage_RejectsOutOfRangeTimes()
    {
        Assert.Throws<ArgumentException>(() => new ReceiveMessageRequest(Url, visibilityTimeout: TimeSpan.FromHours(12.5)));
        Assert.Throws<ArgumentException>(() => new ReceiveMessageRequest(Url, waitTime: TimeSpan.FromSeconds(21)));
        Assert.ThrowsAny<ArgumentException>(() => new ReceiveMessageRequest(""));
    }

    [Fact]
    public void ReceiveMessage_AcceptsTheLimits()
    {
        var request = new ReceiveMessageRequest(Url, 10, visibilityTimeout: TimeSpan.FromHours(12), waitTime: TimeSpan.FromSeconds(20));

        Assert.Equal(10,     request.MaxNumberOfMessages);
        Assert.Equal(43_200, request.VisibilityTimeout);
        Assert.Equal(20,     request.WaitTimeSeconds);
    }

    [Fact]
    public void ReceiveMessage_DefaultsOmitOptionalFields()
    {
        var request = new ReceiveMessageRequest(Url);

        Assert.Null(request.VisibilityTimeout);
        Assert.Null(request.WaitTimeSeconds);

        Assert.Equal(
            """{"QueueUrl":"queue-url","MaxNumberOfMessages":1}""",
            System.Text.Json.JsonSerializer.Serialize(request, SqsSerializerContext.Default.ReceiveMessageRequest));
    }

    [Fact]
    public void SendMessage_RequiresABody()
    {
        Assert.ThrowsAny<ArgumentException>(() => new SendMessageRequest(Url, ""));
        Assert.ThrowsAny<ArgumentException>(() => new SendMessageRequest(Url, null!));
    }

    [Fact]
    public void SendMessageBatch_EnforcesTheBatchLimit()
    {
        var eleven = Enumerable.Range(1, 11).Select(i => i.ToString(CultureInfo.InvariantCulture)).ToArray();

        Assert.Throws<ArgumentException>(() => new SendMessageBatchRequest(Url, (IReadOnlyList<string>)eleven));
        Assert.Throws<ArgumentException>(() => new SendMessageBatchRequest(Url, eleven.Select(i => new SendMessageBatchRequestEntry(i, "body")).ToArray()));

        var ten = new SendMessageBatchRequest(Url, (IReadOnlyList<string>)eleven[..10]);

        Assert.Equal(10, ten.Entries.Length);
        Assert.Equal("10", ten.Entries[9].Id);
    }

    [Fact]
    public void SendMessageBatch_ValidatesArguments()
    {
        Assert.ThrowsAny<ArgumentException>(() => new SendMessageBatchRequest("", (IReadOnlyList<string>)["a"]));
        Assert.Throws<ArgumentNullException>(() => new SendMessageBatchRequest(Url, (IReadOnlyList<string>)null!));
        Assert.ThrowsAny<ArgumentException>(() => new SendMessageBatchRequest("", Array.Empty<SendMessageBatchRequestEntry>()));
        Assert.Throws<ArgumentNullException>(() => new SendMessageBatchRequest(Url, (SendMessageBatchRequestEntry[])null!));
    }

    [Fact]
    public void SendMessageBatchEntry_RequiresIdAndBody()
    {
        Assert.ThrowsAny<ArgumentException>(() => new SendMessageBatchRequestEntry("", "body"));
        Assert.ThrowsAny<ArgumentException>(() => new SendMessageBatchRequestEntry("1", ""));
    }

    [Fact]
    public void SendMessageBatchEntry_KeepsFifoFields()
    {
        var entry = new SendMessageBatchRequestEntry("1", "body", messageDeduplicationId: "dedupe", messageGroupId: "group");

        Assert.Equal("dedupe", entry.MessageDeduplicationId);
        Assert.Equal("group",  entry.MessageGroupId);
        Assert.Null(entry.DelaySeconds);
    }

    [Fact]
    public void DeleteMessageBatch_NumbersReceiptHandlesFromOne()
    {
        string[] handles = ["h1", "h2"];

        var request = new DeleteMessageBatchRequest(Url, handles);

        Assert.Equal(Url, request.QueueUrl);
        Assert.Equal(["1", "2"], request.Entries.Select(e => e.Id));
        Assert.Equal(handles, request.Entries.Select(e => e.ReceiptHandle));
    }

    [Fact]
    public void DeleteMessageBatch_EnforcesTheBatchLimit()
    {
        var handles = Enumerable.Range(1, 11).Select(i => i.ToString(CultureInfo.InvariantCulture)).ToArray();

        Assert.Throws<ArgumentException>(() => new DeleteMessageBatchRequest(Url, handles));
        Assert.Throws<ArgumentException>(() => new DeleteMessageBatchRequest(Url, handles.Select(h => new DeleteMessageBatchRequestEntry(h, h)).ToArray()));

        Assert.Equal(10, new DeleteMessageBatchRequest(Url, handles[..10]).Entries.Length);
    }

    [Fact]
    public void DeleteMessageBatch_ValidatesArguments()
    {
        Assert.ThrowsAny<ArgumentException>(() => new DeleteMessageBatchRequest("", Array.Empty<string>()));
        Assert.Throws<ArgumentNullException>(() => new DeleteMessageBatchRequest(Url, (string[])null!));
        Assert.ThrowsAny<ArgumentException>(() => new DeleteMessageBatchRequest("", Array.Empty<DeleteMessageBatchRequestEntry>()));
        Assert.Throws<ArgumentNullException>(() => new DeleteMessageBatchRequest(Url, (DeleteMessageBatchRequestEntry[])null!));
        Assert.Throws<ArgumentNullException>(() => new DeleteMessageBatchRequestEntry(null!, "handle"));
        Assert.Throws<ArgumentNullException>(() => new DeleteMessageBatchRequestEntry("1", null!));
    }

    [Fact]
    public void DeleteMessage_ValidatesArguments()
    {
        Assert.ThrowsAny<ArgumentException>(() => new DeleteMessageRequest("", "handle"));
        Assert.ThrowsAny<ArgumentException>(() => new DeleteMessageRequest(Url, ""));
    }

    [Fact]
    public void ChangeMessageVisibility_AcceptsTheLimits()
    {
        Assert.Equal(0,      new ChangeMessageVisibilityRequest(Url, "handle", 0).VisibilityTimeout);
        Assert.Equal(43_200, new ChangeMessageVisibilityRequest(Url, "handle", 43_200).VisibilityTimeout);

        Assert.Throws<ArgumentOutOfRangeException>(() => new ChangeMessageVisibilityRequest(Url, "handle", 43_201));
        Assert.ThrowsAny<ArgumentException>(() => new ChangeMessageVisibilityRequest("", "handle", 1));
        Assert.ThrowsAny<ArgumentException>(() => new ChangeMessageVisibilityRequest(Url, "", 1));
    }

    [Fact]
    public void ChangeMessageVisibilityBatch_ValidatesArguments()
    {
        Assert.ThrowsAny<ArgumentException>(() => new ChangeMessageVisibilityBatchRequest("", []));
        Assert.Throws<ArgumentNullException>(() => new ChangeMessageVisibilityBatchRequest(Url, null!));
        Assert.Throws<ArgumentNullException>(() => new ChangeMessageVisibilityBatchRequestEntry(null!, "handle", 1));
        Assert.Throws<ArgumentNullException>(() => new ChangeMessageVisibilityBatchRequestEntry("1", null!, 1));
    }

    [Fact]
    public void ChangeMessageVisibilityBatch_CanSerialize()
    {
        var request = new ChangeMessageVisibilityBatchRequest(Url, [new ChangeMessageVisibilityBatchRequestEntry("1", "handle", 45)]);

        Assert.Equal(
            """{"QueueUrl":"queue-url","Entries":[{"Id":"1","ReceiptHandle":"handle","VisibilityTimeout":45}]}""",
            System.Text.Json.JsonSerializer.Serialize(request, SqsSerializerContext.Default.ChangeMessageVisibilityBatchRequest));
    }

    [Fact]
    public void QueueRequests_ValidateTheirUrl()
    {
        Assert.Throws<ArgumentNullException>(() => new DeleteQueueRequest(null!));
        Assert.ThrowsAny<ArgumentException>(() => new PurgeQueueRequest(" "));
        Assert.ThrowsAny<ArgumentException>(() => new CreateQueueRequest(""));
        Assert.ThrowsAny<ArgumentException>(() => new CreateQueueRequest("", new QueueAttributes()));
    }

    [Fact]
    public void QueueAttributes_SerializeNumbersAsStringsAndOmitNulls()
    {
        var request = new CreateQueueRequest("name", new QueueAttributes {
            DelaySeconds = 1,
            MaximumMessageSize = 2048,
            MessageRetentionPeriod = "60",
            Policy = "p",
            ReceiveMessageWaitTimeSeconds = "5",
            KmsMasterKeyId = "k",
            KmsDataKeyReusePeriodSeconds = "300",
            SqsManagedSseEnabled = "false"
        });

        Assert.Equal(
            """{"QueueName":"name","Attributes":{"DelaySeconds":"1","MaximumMessageSize":"2048","MessageRetentionPeriod":"60","Policy":"p","ReceiveMessageWaitTimeSeconds":"5","KmsMasterKeyId":"k","KmsDataKeyReusePeriodSeconds":"300","SqsManagedSseEnabled":"false"}}""",
            System.Text.Json.JsonSerializer.Serialize(request, SqsSerializerContext.Default.CreateQueueRequest));
    }
}
