using System.Net;

using Carbon.Messaging;

namespace Amazon.Sns.Tests;

public class SnsTopicTests
{
    private static readonly AwsCredential s_credential = new("public", "private");

    private static FakeHandler Success() => new(() => FakeSns.Xml(FakeSns.PublishResponseXml));

    private static SnsTopic CreateTopic(FakeHandler handler, AwsRegion? region = null)
    {
        return FakeSns.Use(new SnsTopic(region ?? AwsRegion.USEast1, "123456789012", "MyTopic", s_credential), handler);
    }

    private static string SentTopicArn(FakeHandler handler)
    {
        return Assert.Single(handler.Requests).Form.Single(p => p.Key == "TopicArn").Value;
    }

    private sealed class StringMessage(string body) : IMessage<string>
    {
        public string Body { get; } = body;
    }

    [Fact]
    public void Constructor_RejectsMissingArguments()
    {
        Assert.Throws<ArgumentNullException>("region", () => new SnsTopic(null!, "123456789012", "MyTopic", s_credential));
        Assert.Throws<ArgumentNullException>("accountId", () => new SnsTopic(AwsRegion.USEast1, null!, "MyTopic", s_credential));
        Assert.Throws<ArgumentException>("accountId", () => new SnsTopic(AwsRegion.USEast1, "", "MyTopic", s_credential));
        Assert.Throws<ArgumentNullException>("topicName", () => new SnsTopic(AwsRegion.USEast1, "123456789012", null!, s_credential));
        Assert.Throws<ArgumentException>("topicName", () => new SnsTopic(AwsRegion.USEast1, "123456789012", "", s_credential));
        Assert.Throws<ArgumentNullException>("credential", () => new SnsTopic(AwsRegion.USEast1, "123456789012", "MyTopic", null!));
    }

    [Theory]
    [InlineData("us-east-1", "arn:aws:sns:us-east-1:123456789012:MyTopic")]
    [InlineData("eu-west-1", "arn:aws:sns:eu-west-1:123456789012:MyTopic")]
    public async Task PublishAsync_BuildsTopicArn(string region, string expected)
    {
        var handler = Success();

        await CreateTopic(handler, new AwsRegion(region)).PublishAsync("Hello");

        Assert.Equal(expected, SentTopicArn(handler));
    }

    // https://docs.aws.amazon.com/IAM/latest/UserGuide/reference-arns.html
    [Theory]
    [InlineData("cn-north-1", "arn:aws-cn:sns:cn-north-1:123456789012:MyTopic")]
    [InlineData("cn-northwest-1", "arn:aws-cn:sns:cn-northwest-1:123456789012:MyTopic")]
    [InlineData("us-gov-west-1", "arn:aws-us-gov:sns:us-gov-west-1:123456789012:MyTopic")]
    [InlineData("us-gov-east-1", "arn:aws-us-gov:sns:us-gov-east-1:123456789012:MyTopic")]
    public async Task PublishAsync_BuildsTopicArn_ForOtherPartitions(string region, string expected)
    {
        var handler = Success();

        await CreateTopic(handler, new AwsRegion(region)).PublishAsync("Hello");

        Assert.Equal(expected, SentTopicArn(handler));
    }

    [Fact]
    public async Task PublishAsync_SendsToRegionalEndpoint()
    {
        var handler = Success();

        await CreateTopic(handler, AwsRegion.USWest2).PublishAsync("Hello");

        Assert.Equal("https://sns.us-west-2.amazonaws.com/", handler.Requests[0].RequestUri.ToString());
    }

    [Fact]
    public async Task PublishAsync_String_SendsMessage_AndReturnsResult()
    {
        var handler = Success();

        var result = await CreateTopic(handler).PublishAsync("Hello");

        Assert.Single(handler.Requests[0].Form, p => p.Key == "Message" && p.Value == "Hello");
        Assert.DoesNotContain(handler.Requests[0].Form, p => p.Key == "Subject");
        Assert.Equal("94f20ce6-13c5-43a0-9a9e-ca52d816e90b", result.MessageId);
    }

    [Fact]
    public async Task PublishAsync_String_SendsSubject()
    {
        var handler = Success();

        await CreateTopic(handler).PublishAsync("Hello", "Subject");

        Assert.Single(handler.Requests[0].Form, p => p.Key == "Subject" && p.Value == "Subject");
    }

    [Fact]
    public void TopicArn_IsExposed()
    {
        Assert.Equal("arn:aws:sns:us-east-1:123456789012:MyTopic", CreateTopic(Success()).TopicArn);
    }

    [Fact]
    public void PublishAsync_RejectsNullMessage()
    {
        Assert.Throws<ArgumentNullException>(() => { _ = CreateTopic(Success()).PublishAsync((IMessage<string>)null!); });
    }

    [Fact]
    public async Task PublishAsync_Message_SendsBody()
    {
        var handler = Success();

        var result = await CreateTopic(handler).PublishAsync(new StringMessage("From a message"));

        Assert.Equal("94f20ce6-13c5-43a0-9a9e-ca52d816e90b", result.MessageId);

        Assert.Single(handler.Requests[0].Form, p => p.Key == "Message" && p.Value == "From a message");
    }

    [Fact]
    public void PublishAsync_RejectsEmptyMessage_WithoutSending()
    {
        var handler = Success();
        var topic = CreateTopic(handler);

        Assert.Throws<ArgumentException>(() => { _ = topic.PublishAsync(""); });
        Assert.Throws<ArgumentException>(() => { _ = topic.PublishAsync(new StringMessage("")); });

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task PublishAsync_Message_PropagatesFailure()
    {
        var handler = new FakeHandler(() => FakeSns.Xml(FakeSns.NotFoundErrorXml, HttpStatusCode.NotFound));

        var ex = await Assert.ThrowsAsync<SnsException>(() => CreateTopic(handler).PublishAsync(new StringMessage("Hello")));

        Assert.Equal("NotFound", ex.Code);
    }
}
