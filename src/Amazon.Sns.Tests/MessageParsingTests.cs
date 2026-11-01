using System.Text.Json;

namespace Amazon.Sns.Tests;

public class MessageParsingTests
{
    // Everything a message requires, other than Type
    private const string CommonFields =
        """
        "MessageId" : "22b80b92-fdea-4c2c-8f9d-bdfb0c7bf324",
        "TopicArn" : "arn:aws:sns:us-west-2:123456789012:MyTopic",
        "Message" : "Hello world!",
        "Timestamp" : "2012-05-02T00:54:06.655Z",
        "SignatureVersion" : "1",
        "Signature" : "EXAMPLEw6JRN...",
        "SigningCertURL" : "https://sns.us-west-2.amazonaws.com/cert.pem"
        """;

    private static SnsMessage? Parse(string json) => JsonSerializer.Deserialize<SnsMessage>(json);

    [Fact]
    public void Notification_SubjectAndUnsubscribeUrl_AreOptional()
    {
        var message = Assert.IsType<Notification>(Parse($$"""{ "Type" : "Notification", {{CommonFields}} }"""));

        Assert.Null(message.Subject);
        Assert.Null(message.UnsubscribeURL);
    }

    [Fact]
    public void Notification_ParsesMessageAttributes()
    {
        var message = Parse(
            $$"""
            {
              "Type" : "Notification",
              {{CommonFields}},
              "MessageAttributes" : { "color" : { "Type" : "String", "Value" : "red" } }
            }
            """);

        var attribute = Assert.Single(Assert.IsType<Notification>(message).MessageAttributes!);

        Assert.Equal("color",  attribute.Key);
        Assert.Equal("String", attribute.Value.Type);
        Assert.Equal("red",    attribute.Value.Value);
    }

    [Fact]
    public void SupportsSignatureVersion2()
    {
        var message = Parse(
            """
            {
              "Type" : "Notification",
              "MessageId" : "1",
              "TopicArn" : "arn:aws:sns:us-west-2:123456789012:MyTopic",
              "Message" : "Hello",
              "Timestamp" : "2012-05-02T00:54:06.655Z",
              "SignatureVersion" : "2",
              "Signature" : "abc",
              "SigningCertURL" : "https://sns.us-west-2.amazonaws.com/cert.pem"
            }
            """);

        Assert.Equal("2", message!.SignatureVersion);
    }

    [Fact]
    public void Timestamp_IsUtc()
    {
        var message = Parse($$"""{ "Type" : "Notification", {{CommonFields}} }""");

        Assert.Equal(new DateTimeOffset(2012, 5, 2, 0, 54, 6, 655, TimeSpan.Zero), message!.Timestamp);
        Assert.Equal(TimeSpan.Zero, message.Timestamp.Offset);
    }

    [Theory]
    [InlineData("MessageId")]
    [InlineData("TopicArn")]
    [InlineData("Message")]
    [InlineData("Timestamp")]
    [InlineData("SignatureVersion")]
    [InlineData("Signature")]
    [InlineData("SigningCertURL")]
    public void MissingRequiredField_Throws(string field)
    {
        var fields = CommonFields
            .Split('\n')
            .Select(static line => line.Trim().TrimEnd(','))
            .Where(line => !line.StartsWith($"\"{field}\""));

        var json = $$"""{ "Type" : "Notification", {{string.Join(",", fields)}} }""";

        var ex = Assert.Throws<JsonException>(() => Parse(json));

        Assert.Contains(field, ex.Message);
    }

    [Theory]
    [InlineData("SubscriptionConfirmation")]
    [InlineData("UnsubscribeConfirmation")]
    public void Confirmations_RequireSubscribeUrlAndToken(string type)
    {
        var ex = Assert.Throws<JsonException>(() => Parse($$"""{ "Type" : "{{type}}", {{CommonFields}} }"""));

        Assert.Contains("SubscribeURL", ex.Message);
        Assert.Contains("Token", ex.Message);
    }

    [Fact]
    public void UnknownType_Throws()
    {
        Assert.Throws<JsonException>(() => Parse($$"""{ "Type" : "Other", {{CommonFields}} }"""));
    }

    [Fact]
    public void MissingType_Throws()
    {
        Assert.Throws<NotSupportedException>(() => Parse($$"""{ {{CommonFields}} }"""));
    }

    // System.Text.Json only reads the discriminator when it comes first.
    // The AWS examples all start with Type, but the docs do not promise an order.
    [Fact]
    public void TypeAfterOtherFields_Throws()
    {
        Assert.Throws<NotSupportedException>(() => Parse($$"""{ {{CommonFields}}, "Type" : "Notification" }"""));
    }

    [Fact]
    public void Serialize_AsSnsMessage_RoundTrips()
    {
        SnsMessage message = new SubscriptionConfirmation {
            MessageId        = "1",
            TopicArn         = "arn:aws:sns:us-west-2:123456789012:MyTopic",
            Subject          = "Subject",
            Message          = "Hello",
            Timestamp        = new DateTimeOffset(2012, 5, 2, 0, 54, 6, TimeSpan.Zero),
            SignatureVersion = "1",
            Signature        = "abc",
            SigningCertURL   = "https://sns.us-west-2.amazonaws.com/cert.pem",
            SubscribeURL     = "https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription",
            Token            = "token"
        };

        var json = JsonSerializer.Serialize(message);

        Assert.StartsWith("""{"Type":"SubscriptionConfirmation",""", json);

        var result = Assert.IsType<SubscriptionConfirmation>(Parse(json));

        Assert.Equal(message.MessageId,        result.MessageId);
        Assert.Equal(message.TopicArn,         result.TopicArn);
        Assert.Equal(message.Subject,          result.Subject);
        Assert.Equal(message.Message,          result.Message);
        Assert.Equal(message.Timestamp,        result.Timestamp);
        Assert.Equal(message.SignatureVersion, result.SignatureVersion);
        Assert.Equal(message.Signature,        result.Signature);
        Assert.Equal(message.SigningCertURL,   result.SigningCertURL);
        Assert.Equal("https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription", result.SubscribeURL);
        Assert.Equal("token", result.Token);
    }

    [Theory]
    [InlineData(MessageType.Notification, "Notification")]
    [InlineData(MessageType.SubscriptionConfirmation, "SubscriptionConfirmation")]
    [InlineData(MessageType.UnsubscribeConfirmation, "UnsubscribeConfirmation")]
    public void MessageType_SerializesAsString(MessageType type, string name)
    {
        Assert.Equal($"\"{name}\"", JsonSerializer.Serialize(type));
        Assert.Equal(type, JsonSerializer.Deserialize<MessageType>($"\"{name}\""));
    }
}
