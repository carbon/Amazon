using System.Text.Json;

namespace Amazon.Sns.Tests;

public class NotificationTests
{
    [Fact]
    public void CanDeserialize()
    {
        var message = JsonSerializer.Deserialize<SnsMessage>(
            """
            {
              "Type" : "Notification",
              "MessageId" : "22b80b92-fdea-4c2c-8f9d-bdfb0c7bf324",
              "TopicArn" : "arn:aws:sns:us-west-2:123456789012:MyTopic",
              "Subject" : "My First Message",
              "Message" : "Hello world!",
              "Timestamp" : "2012-05-02T00:54:06.655Z",
              "SignatureVersion" : "1",
              "Signature" : "EXAMPLEw6JRN...",
              "SigningCertURL" : "https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem",
              "UnsubscribeURL" : "https://sns.us-west-2.amazonaws.com/?Action=Unsubscribe&SubscriptionArn=arn:aws:sns:us-west-2:123456789012:MyTopic:c9135db0-26c4-47ec-8998-413945fb5a96"
            }
            """);

        Assert.NotNull(message);
        Assert.Equal("22b80b92-fdea-4c2c-8f9d-bdfb0c7bf324",        message.MessageId);
        Assert.Equal("arn:aws:sns:us-west-2:123456789012:MyTopic",  message.TopicArn);
        Assert.Equal("EXAMPLEw6JRN...",                             message.Signature);
        Assert.Equal("1",                                           message.SignatureVersion);

        var notification = (Notification)message;

        Assert.Equal("Hello world!", notification.Message);
    }
}