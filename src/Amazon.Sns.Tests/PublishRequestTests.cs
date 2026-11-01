namespace Amazon.Sns.Tests;

public class PublishRequestTests
{
    private const string TopicArn = "arn:aws:sns:us-east-1:123456789012:MyTopic";

    [Fact]
    public void Constructor_SetsProperties()
    {
        var request = new PublishRequest(TopicArn, "Hello", "Subject");

        Assert.Equal(TopicArn,  request.TopicArn);
        Assert.Equal("Hello",   request.Message);
        Assert.Equal("Subject", request.Subject);
    }

    [Fact]
    public void Subject_IsOptional()
    {
        Assert.Null(new PublishRequest(TopicArn, "Hello").Subject);
    }

    [Fact]
    public void Constructor_RejectsMissingTopicArn()
    {
        Assert.Throws<ArgumentNullException>("topicArn", () => new PublishRequest(null!, "Hello"));
        Assert.Throws<ArgumentException>("topicArn", () => new PublishRequest("", "Hello"));
    }

    [Fact]
    public void Constructor_RejectsMissingMessage()
    {
        Assert.Throws<ArgumentNullException>("message", () => new PublishRequest(TopicArn, null!));
        Assert.Throws<ArgumentException>("message", () => new PublishRequest(TopicArn, ""));
    }
}
