using System.Net;
using System.Net.Http;

namespace Amazon.Sns.Tests;

public class SnsClientTests
{
    private const string TopicArn = "arn:aws:sns:us-east-1:123456789012:MyTopic";

    private static SnsClient CreateClient(FakeHandler handler)
    {
        return FakeSns.Use(new SnsClient(AwsRegion.USEast1, new AwsCredential("public", "private")), handler);
    }

    private static FakeHandler Success() => new(() => FakeSns.Xml(FakeSns.PublishResponseXml));

    [Fact]
    public void Constructor_RejectsNullArguments()
    {
        Assert.Throws<ArgumentNullException>(() => new SnsClient(null!, new AwsCredential("public", "private")));
        Assert.Throws<ArgumentNullException>(() => new SnsClient(AwsRegion.USEast1, null!));
    }

    [Fact]
    public void Endpoint_IncludesRegion()
    {
        var client = new SnsClient(AwsRegion.USWest2, new AwsCredential("public", "private"));

        Assert.Equal("https://sns.us-west-2.amazonaws.com/", client.Endpoint);
        Assert.Equal(AwsRegion.USWest2, client.Region);
    }

    [Fact]
    public async Task PublishAsync_PostsSignedForm()
    {
        var handler = Success();

        await CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello world!"));

        var request = Assert.Single(handler.Requests);

        Assert.Same(HttpMethod.Post, request.Method);
        Assert.Equal("https://sns.us-east-1.amazonaws.com/", request.RequestUri.ToString());
        Assert.StartsWith("application/x-www-form-urlencoded", request.ContentType);
        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", request.Authorization);
        Assert.Contains("/us-east-1/sns/aws4_request", request.Authorization);

        Assert.Equal(
        [
            new("Action", "Publish"),
            new("TopicArn", TopicArn),
            new("Message", "Hello world!"),
            new("Version", "2010-03-31")
        ], request.Form);
    }

    [Fact]
    public async Task PublishAsync_IncludesSubject()
    {
        var handler = Success();

        await CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello", subject: "My first message"));

        Assert.Equal(
        [
            new("Action", "Publish"),
            new("TopicArn", TopicArn),
            new("Message", "Hello"),
            new("Subject", "My first message"),
            new("Version", "2010-03-31")
        ], handler.Requests[0].Form);
    }

    [Theory]
    [InlineData("a&b=c+d e%20f")]
    [InlineData("line1\nline2")]
    [InlineData("""{"default":"text"}""")]
    [InlineData("héllo wörld ✓ 🎉")]
    public async Task PublishAsync_EncodesMessage(string message)
    {
        var handler = Success();

        await CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, message));

        Assert.Single(handler.Requests[0].Form, p => p.Key == "Message" && p.Value == message);
    }

    [Fact]
    public async Task PublishAsync_SendsOptions()
    {
        var handler = Success();

        await CreateClient(handler).PublishAsync(new PublishRequest(TopicArn + ".fifo", "{}", "Subject") {
            MessageStructure       = "json",
            MessageGroupId         = "group",
            MessageDeduplicationId = "dedupe",
            MessageAttributes      = new Dictionary<string, MessageAttributeValue> {
                ["color"] = MessageAttributeValue.FromString("red"),
                ["size"]  = MessageAttributeValue.FromNumber(1.5m),
                ["tags"]  = MessageAttributeValue.FromStringArray("""["a","b"]"""),
                ["blob"]  = MessageAttributeValue.FromBinary([1, 2, 3])
            }
        });

        Assert.Equal(
        [
            new("Action", "Publish"),
            new("TopicArn", TopicArn + ".fifo"),
            new("Message", "{}"),
            new("Subject", "Subject"),
            new("MessageStructure", "json"),
            new("MessageGroupId", "group"),
            new("MessageDeduplicationId", "dedupe"),
            new("MessageAttributes.entry.1.Name", "color"),
            new("MessageAttributes.entry.1.Value.DataType", "String"),
            new("MessageAttributes.entry.1.Value.StringValue", "red"),
            new("MessageAttributes.entry.2.Name", "size"),
            new("MessageAttributes.entry.2.Value.DataType", "Number"),
            new("MessageAttributes.entry.2.Value.StringValue", "1.5"),
            new("MessageAttributes.entry.3.Name", "tags"),
            new("MessageAttributes.entry.3.Value.DataType", "String.Array"),
            new("MessageAttributes.entry.3.Value.StringValue", """["a","b"]"""),
            new("MessageAttributes.entry.4.Name", "blob"),
            new("MessageAttributes.entry.4.Value.DataType", "Binary"),
            new("MessageAttributes.entry.4.Value.BinaryValue", "AQID"),
            new("Version", "2010-03-31")
        ], handler.Requests[0].Form);
    }

    [Fact]
    public async Task PublishAsync_RejectsNullRequest()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => CreateClient(Success()).PublishAsync(null!));
    }

    [Fact]
    public async Task PublishAsync_ParsesResult()
    {
        var result = await CreateClient(Success()).PublishAsync(new PublishRequest(TopicArn, "Hello"));

        Assert.Equal("94f20ce6-13c5-43a0-9a9e-ca52d816e90b", result.MessageId);
        Assert.Null(result.SequenceNumber);
    }

    [Fact]
    public async Task PublishAsync_ParsesSequenceNumber()
    {
        var handler = new FakeHandler(() => FakeSns.Xml(
            """
            <PublishResponse xmlns="https://sns.amazonaws.com/doc/2010-03-31/">
              <PublishResult>
                <SequenceNumber>10000000000000003000</SequenceNumber>
                <MessageId>abc</MessageId>
              </PublishResult>
            </PublishResponse>
            """));

        var result = await CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello"));

        Assert.Equal("abc", result.MessageId);
        Assert.Equal("10000000000000003000", result.SequenceNumber);
    }

    [Fact]
    public async Task PublishAsync_ThrowsWhenResultIsMissingMessageId()
    {
        var handler = new FakeHandler(() => FakeSns.Xml("<PublishResponse />"));

        await Assert.ThrowsAsync<FormatException>(() => CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello")));
    }

    [Fact]
    public async Task PublishAsync_HonorsCancellation()
    {
        var handler = Success();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello"), new CancellationToken(canceled: true)));

        Assert.Empty(handler.Requests);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task PublishAsync_ThrowsParsedError_WithoutRetrying(HttpStatusCode statusCode)
    {
        var handler = new FakeHandler(() => FakeSns.Xml(FakeSns.NotFoundErrorXml, statusCode));

        var ex = await Assert.ThrowsAsync<SnsException>(
            () => CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello")));

        Assert.Equal("Topic does not exist", ex.Message);
        Assert.Equal("NotFound", ex.Code);
        Assert.Equal("f187a3c1-376f-11df-8963-01868b7c937a", ex.RequestId);
        Assert.Equal(statusCode, ex.HttpStatusCode);
        Assert.False(ex.IsTransient);
        Assert.Single(handler.Requests);
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    [InlineData(HttpStatusCode.TooManyRequests)]
    public async Task PublishAsync_RetriesTransientStatus_ThenThrows(HttpStatusCode statusCode)
    {
        var handler = new FakeHandler(() => FakeSns.Xml(FakeSns.NotFoundErrorXml, statusCode));

        var ex = await Assert.ThrowsAsync<SnsException>(
            () => CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello")));

        Assert.True(ex.IsTransient);
        Assert.Equal(3, handler.Requests.Count);
    }

    [Fact]
    public async Task PublishAsync_RetriesThrottling_ThenSucceeds()
    {
        var handler = new FakeHandler(
            () => FakeSns.Xml(FakeSns.NotFoundErrorXml.Replace("NotFound", "ThrottlingException"), HttpStatusCode.BadRequest),
            () => FakeSns.Xml(FakeSns.PublishResponseXml));

        var result = await CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello"));

        Assert.Equal("94f20ce6-13c5-43a0-9a9e-ca52d816e90b", result.MessageId);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(handler.Requests[0].Body, handler.Requests[1].Body);
    }

    [Fact]
    public async Task PublishAsync_UsesBody_WhenErrorIsNotXml()
    {
        var handler = new FakeHandler(() => FakeSns.Xml("Bad Gateway", HttpStatusCode.BadGateway));

        var ex = await Assert.ThrowsAsync<SnsException>(
            () => CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello")));

        Assert.Equal("Bad Gateway", ex.Message);
        Assert.Null(ex.Code);
        Assert.True(ex.IsTransient);
    }

    [Fact]
    public async Task ConfirmSubscriptionAsync_PostsForm_AndParsesResult()
    {
        var handler = new FakeHandler(() => FakeSns.Xml(
            """
            <ConfirmSubscriptionResponse xmlns="https://sns.amazonaws.com/doc/2010-03-31/">
                <ConfirmSubscriptionResult>
                    <SubscriptionArn>arn:aws:sns:us-east-2:123456789012:My-Topic:80289ba6</SubscriptionArn>
                </ConfirmSubscriptionResult>
                <ResponseMetadata>
                    <RequestId>7a50221f-3774-11df-a9b7-05d48da6f042</RequestId>
                </ResponseMetadata>
            </ConfirmSubscriptionResponse>
            """));

        var result = await CreateClient(handler).ConfirmSubscriptionAsync(new ConfirmSubscriptionRequest(TopicArn, "token"));

        Assert.Equal("arn:aws:sns:us-east-2:123456789012:My-Topic:80289ba6", result.SubscriptionArn);

        Assert.Equal(
        [
            new("Action", "ConfirmSubscription"),
            new("TopicArn", TopicArn),
            new("Token", "token"),
            new("Version", "2010-03-31")
        ], handler.Requests[0].Form);
    }

    [Fact]
    public async Task ConfirmSubscriptionAsync_SendsAuthenticateOnUnsubscribe()
    {
        var handler = new FakeHandler(() => FakeSns.Xml("<R><SubscriptionArn>arn</SubscriptionArn></R>"));

        await CreateClient(handler).ConfirmSubscriptionAsync(new ConfirmSubscriptionRequest(TopicArn, "token", authenticateOnUnsubscribe: true));

        Assert.Contains(new("AuthenticateOnUnsubscribe", "true"), handler.Requests[0].Form);
    }

    [Fact]
    public async Task ConfirmSubscriptionAsync_ThrowsWhenResultIsMissingArn()
    {
        var handler = new FakeHandler(() => FakeSns.Xml("<R />"));

        await Assert.ThrowsAsync<FormatException>(
            () => CreateClient(handler).ConfirmSubscriptionAsync(new ConfirmSubscriptionRequest(TopicArn, "token")));
    }

    [Fact]
    public async Task ConfirmSubscriptionAsync_RejectsNullRequest()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => CreateClient(Success()).ConfirmSubscriptionAsync(null!));
    }
}
