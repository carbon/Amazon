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
    public void ConfirmSubscriptionRequest_RejectsMissingArguments()
    {
        Assert.Throws<ArgumentNullException>("topicArn", () => new ConfirmSubscriptionRequest(null!, "token"));
        Assert.Throws<ArgumentException>("topicArn", () => new ConfirmSubscriptionRequest("", "token"));
        Assert.Throws<ArgumentNullException>("token", () => new ConfirmSubscriptionRequest(TopicArn, null!));
        Assert.Throws<ArgumentException>("token", () => new ConfirmSubscriptionRequest(TopicArn, ""));
    }

    [Fact]
    public void MessageAttributeValue_RejectsEmptyValues()
    {
        Assert.Throws<ArgumentException>(() => MessageAttributeValue.FromString(""));
        Assert.Throws<ArgumentException>(() => MessageAttributeValue.FromStringArray(""));
        Assert.Throws<ArgumentNullException>(() => MessageAttributeValue.FromBinary(null!));
        Assert.Throws<ArgumentException>(() => MessageAttributeValue.FromBinary([]));
    }

    [Fact]
    public void MessageAttributeValue_FormatsNumbersInvariantly()
    {
        var culture = System.Globalization.CultureInfo.CurrentCulture;

        try
        {
            System.Globalization.CultureInfo.CurrentCulture = new("de-DE");

            Assert.Equal("1.5", MessageAttributeValue.FromNumber(1.5m).StringValue);
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentCulture = culture;
        }
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
