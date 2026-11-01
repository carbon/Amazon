using System.Text.Json;

namespace Amazon.Ses.Notifications.Tests;

public class BounceNotificationTests
{
    [Fact]
    public void CanDeserialize()
    {
        var notification = JsonSerializer.Deserialize<SesNotification>(
            """
            {
               "notificationType":"Bounce",
               "bounce":{
                  "bounceType":"Permanent",
                  "reportingMTA":"dns; email.example.com",
                  "bouncedRecipients":[
                     {
                        "emailAddress":"jane@example.com",
                        "status":"5.1.1",
                        "action":"failed",
                        "diagnosticCode":"smtp; 550 5.1.1 <jane@example.com>... User"
                     }
                  ],
                  "bounceSubType":"General",
                  "timestamp":"2016-01-27T14:59:38.237Z",
                  "feedbackId":"00000138111222aa-33322211-cccc-cccc-cccc-ddddaaaa068a-000000",
                  "remoteMtaIp":"127.0.2.0"
               },
               "mail":{
                  "timestamp":"2016-01-27T14:59:38.237Z",
                  "source":"john@example.com",
                  "sourceArn": "arn:aws:ses:us-east-1:888888888888:identity/example.com",
                  "sourceIp": "127.0.3.0",
                  "sendingAccountId":"123456789012",
                  "callerIdentity": "IAM_user_or_role_name",
                  "messageId":"00000138111222aa-33322211-cccc-cccc-cccc-ddddaaaa0680-000000"
                }
            }
            """);


        Assert.Equal(SesNotificationType.Bounce, notification.NotificationType);
        Assert.Equal(SesBounceType.Permanent, notification.Bounce.BounceType);
        Assert.Equal("00000138111222aa-33322211-cccc-cccc-cccc-ddddaaaa068a-000000", notification.Bounce.FeedbackId);
        Assert.Equal("00000138111222aa-33322211-cccc-cccc-cccc-ddddaaaa0680-000000", notification.Mail.MessageId);
    }
}
