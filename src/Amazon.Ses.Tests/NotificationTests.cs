using System.Text.Json;

namespace Amazon.Ses.Tests;

public class NotificationTests
{
    [Fact]
    public void ParseComplaint()
    {
        var text = @"{
			""notificationType"":""Complaint"",
			""complaint"":{
				""complainedRecipients"":[
				{
					""emailAddress"":""recipient1@example.com""
				}
				],
				""timestamp"":""2012-05-25T14:59:38.613-07:00"",
				""feedbackId"":""0000013786031775-fea503bc-7497-49e1-881b-a0379bb037d3-000000""
			},
			""mail"":{
				""timestamp"":""2012-05-25T14:59:38.613-07:00"",
				""messageId"":""0000013786031775-163e3910-53eb-4c8e-a04a-f29debf88a84-000000"",
				""source"":""email_1337983178613@amazon.com"",
				""destination"":[
				""recipient1@example.com"",
				""recipient2@example.com"",
				""recipient3@example.com"",
				""recipient4@example.com""
				]
			}
		}";

        var notification = JsonSerializer.Deserialize<SesNotification>(text);

        var complaint = notification.Complaint;
        var mail = notification.Mail;

        Assert.Equal(SesNotificationType.Complaint,                                  notification.NotificationType);
        Assert.Equal(2012,                                                           complaint.Timestamp.Year);
        Assert.Equal(-7,                                                             complaint.Timestamp.Offset.Hours);
        Assert.Equal("0000013786031775-fea503bc-7497-49e1-881b-a0379bb037d3-000000", complaint.FeedbackId);


        Assert.Equal("recipient1@example.com", complaint.ComplainedRecipients[0].EmailAddress);

        Assert.Equal(4, mail.Destination.Length);

        Assert.Equal("recipient1@example.com", mail.Destination[0]);
        Assert.Equal("recipient2@example.com", mail.Destination[1]);

        Assert.Equal("email_1337983178613@amazon.com", mail.Source);
        Assert.Equal("0000013786031775-163e3910-53eb-4c8e-a04a-f29debf88a84-000000", mail.MessageId);
    }

    [Fact]
    public void ParseBounce()
    {
        var text = @"{""notificationType"":""Bounce"",""bounce"":{""bounceSubType"":""Suppressed"",""bounceType"":""Permanent"",""bouncedRecipients"":[{""action"":""failed"",""diagnosticCode"":""Amazon SES has suppressed sending to this address because it has a recent history of bouncing as an invalid address. For more information about how to remove an address from the suppression list, see the Amazon SES Developer Guide: http://docs.aws.amazon.com/ses/latest/DeveloperGuide/remove-from-suppressionlist.html"",""status"":""5.1.1"",""emailAddress"":""hi@simulator.amazonses.com""}],""reportingMTA"":""dns; amazonses.com"",""timestamp"":""2014-01-14T07:24:56.332Z"",""feedbackId"":""000001438fa38a49-f7cb046b-7cec-11e3-b22a-31634d3963ed-000000""},""mail"":{""timestamp"":""2014-01-14T07:24:53.000Z"",""destination"":[""hi@simulator.amazonses.com""],""messageId"":""000001438fa37fa1-08180675-1d15-4bfc-b388-3b71a75f5a31-000000"",""source"":""hello@carbonmade.com""}}";

        var notification = JsonSerializer.Deserialize<SesNotification>(text);

        var mail = notification.Mail;

        Assert.Equal(SesNotificationType.Bounce, notification.NotificationType);

        Assert.Equal(SesBounceType.Permanent, notification.Bounce.BounceType);
        Assert.Equal(SesBounceSubtype.Suppressed, notification.Bounce.BounceSubType);

        Assert.Equal("Amazon SES has suppressed sending to this address because it has a recent history of bouncing as an invalid address. For more information about how to remove an address from the suppression list, see the Amazon SES Developer Guide: http://docs.aws.amazon.com/ses/latest/DeveloperGuide/remove-from-suppressionlist.html", notification.Bounce.BouncedRecipients[0].DiagnosticCode);

        Assert.Equal("hi@simulator.amazonses.com", mail.Destination[0]);
        Assert.Equal("hi@simulator.amazonses.com", notification.Bounce.BouncedRecipients[0].EmailAddress);

        Assert.Equal("000001438fa37fa1-08180675-1d15-4bfc-b388-3b71a75f5a31-000000", mail.MessageId);
    }
}
