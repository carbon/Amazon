using System.Text.Json;

namespace Amazon.Sns.Tests;

public class UnsubscribeConfirmationTests
{
    [Fact]
    public void CanDeserialize()
    {
        var notification = JsonSerializer.Deserialize<SnsMessage>(
            """                        
            {
              "Type" : "UnsubscribeConfirmation",
              "MessageId" : "47138184-6831-46b8-8f7c-afc488602d7d",
              "Token" : "2336412f37...",
              "TopicArn" : "arn:aws:sns:us-west-2:123456789012:MyTopic",
              "Message" : "You have chosen to deactivate subscription arn:aws:sns:us-west-2:123456789012:MyTopic:2bcfbf39-05c3-41de-beaa-fcfcc21c8f55.\nTo cancel this operation and restore the subscription, visit the SubscribeURL included in this message.",
              "SubscribeURL" : "https://sns.us-west-2.amazonaws.com/?Action=ConfirmSubscription&TopicArn=arn:aws:sns:us-west-2:123456789012:MyTopic&Token=2336412f37fb6...",
              "Timestamp" : "2012-04-26T20:06:41.581Z",
              "SignatureVersion" : "1",
              "Signature" : "EXAMPLEHXgJm...",
              "SigningCertURL" : "https://sns.us-west-2.amazonaws.com/SimpleNotificationService-f3ecfb7224c7233fe7bb5f59f96de52f.pem"
            }
            """);

        Assert.True(notification is UnsubscribeConfirmation);

        Assert.Equal("47138184-6831-46b8-8f7c-afc488602d7d", notification.MessageId);
    }
}
