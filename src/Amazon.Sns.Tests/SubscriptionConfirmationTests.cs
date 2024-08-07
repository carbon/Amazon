using System.Text.Json;

namespace Amazon.Sns.Tests;

public class SubscriptionConfirmationTests
{
    [Fact]
    public void CanDeserialize()
    {
        var notification = JsonSerializer.Deserialize<SnsMessage>(
            """
            {
              "Type" : "SubscriptionConfirmation",
              "MessageId" : "165545c9-2a5c-472c-8df2-7ff2be2b3b1b",
              "Token" : "2336412f37f...",
              "TopicArn" : "arn:aws:sns:us-west-2:123456789012:MyTopic",
              "Message" : "You have chosen to subscribe to the topic arn:aws:sns:us-west-2:123456789012:MyTopic.\nTo confirm the subscription, visit the SubscribeURL included in this message.",
              "SubscribeURL" : "https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:us-west-2:123456789012:MyTopic&Token=2336412f37...",
              "Timestamp" : "2012-04-26T20:45:04.751Z",
              "SignatureVersion" : "1",
              "Signature" : "EXAMPLEpH+...",
              "SigningCertURL" : "https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem"
            }
            """u8);

        var confirmation = (SubscriptionConfirmation)notification!;

        Assert.Equal("165545c9-2a5c-472c-8df2-7ff2be2b3b1b", confirmation.MessageId);
        Assert.Equal("arn:aws:sns:us-west-2:123456789012:MyTopic", confirmation.TopicArn);
        Assert.Equal("https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:us-west-2:123456789012:MyTopic&Token=2336412f37...", confirmation.SubscribeURL);
        Assert.Equal("https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem", confirmation.SigningCertURL);
    }


    [Fact]
    public void CanDeserialize2()
    {
        var confirmation = JsonSerializer.Deserialize<SubscriptionConfirmation>(
            """
            {
              "Type" : "SubscriptionConfirmation",
              "MessageId" : "165545c9-2a5c-472c-8df2-7ff2be2b3b1b",
              "Token" : "2336412f37f...",
              "TopicArn" : "arn:aws:sns:us-west-2:123456789012:MyTopic",
              "Message" : "You have chosen to subscribe to the topic arn:aws:sns:us-west-2:123456789012:MyTopic.\nTo confirm the subscription, visit the SubscribeURL included in this message.",
              "SubscribeURL" : "https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:us-west-2:123456789012:MyTopic&Token=2336412f37...",
              "Timestamp" : "2012-04-26T20:45:04.751Z",
              "SignatureVersion" : "1",
              "Signature" : "EXAMPLEpH+...",
              "SigningCertURL" : "https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem"
            }
            """u8);

        Assert.NotNull(confirmation);
        Assert.Equal("165545c9-2a5c-472c-8df2-7ff2be2b3b1b", confirmation.MessageId);
        Assert.Equal("arn:aws:sns:us-west-2:123456789012:MyTopic", confirmation.TopicArn);
        Assert.Equal("https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:us-west-2:123456789012:MyTopic&Token=2336412f37...", confirmation.SubscribeURL);
        Assert.Equal("https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem", confirmation.SigningCertURL);
    }
}
