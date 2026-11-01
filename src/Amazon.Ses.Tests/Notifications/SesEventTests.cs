using System.Text.Json;

using Amazon.Ses.Serialization;

namespace Amazon.Ses.Notifications.Tests;

public class SesEventTests
{
    [Theory]
    [InlineData("Send", SesEventType.Send)]
    [InlineData("Reject", SesEventType.Reject)]
    [InlineData("Delivery", SesEventType.Delivery)]
    [InlineData("Bounce", SesEventType.Bounce)]
    [InlineData("DeliveryDelay", SesEventType.DeliveryDelay)]
    [InlineData("Complaint", SesEventType.Complaint)]
    [InlineData("Open", SesEventType.Open)]
    [InlineData("Click", SesEventType.Click)]
    [InlineData("RenderingFailure", SesEventType.RenderingFailure)]
    [InlineData("Subscription", SesEventType.Subscription)]
    public void CanDeserializeEventType(string name, SesEventType expected)
    {
        var result = JsonSerializer.Deserialize<SesEvent>($$"""{"eventType":"{{name}}"}""");

        Assert.Equal(expected, result!.EventType);
    }

    [Fact]
    public void CanDeserializeDeliveryEvent()
    {
        var result = JsonSerializer.Deserialize<SesEvent>(
            """
            {
              "eventType": "Delivery",
              "mail": { "messageId": "abc", "source": "from@test.com", "destination": ["to@test.com"] },
              "delivery": {
                "timestamp": "2016-10-19T23:21:04.133Z",
                "processingTimeMillis": 11893,
                "recipients": ["to@test.com"],
                "smtpResponse": "250 2.6.0 Message received",
                "reportingMTA": "mta.example.com",
                "remoteMtaIp": "10.0.0.1"
              }
            }
            """);

        Assert.Equal(SesEventType.Delivery, result!.EventType);
        Assert.Equal("abc", result.Mail.MessageId);
        Assert.Equal("10.0.0.1", result.Delivery!.RemoteMtaIp);
    }

    [Fact]
    public void CanDeserializeCommonHeadersAndContent()
    {
        var result = JsonSerializer.Deserialize(
            """
            {
              "notificationType": "Received",
              "content": "raw message",
              "receipt": { "dmarcPolicy": "reject" },
              "mail": {
                "messageId": "abc",
                "commonHeaders": {
                  "messageId": "<1@test.com>",
                  "date": "Mon, 1 Jan 2024 00:00:00 +0000",
                  "from": ["from@test.com"],
                  "sender": "sender@test.com",
                  "returnPath": "bounces@test.com",
                  "reply-to": ["reply@test.com"],
                  "to": ["to@test.com"],
                  "cc": ["cc@test.com"],
                  "bcc": ["bcc@test.com"],
                  "subject": "Subject"
                }
              }
            }
            """, SesSerializerContext.Default.SesNotification);

        Assert.Equal("raw message", result!.Content);
        Assert.Equal("reject", result.Receipt!.DmarcPolicy);

        var headers = result.Mail.CommonHeaders!;

        Assert.Equal("sender@test.com",  headers.Sender);
        Assert.Equal("bounces@test.com", headers.ReturnPath);
        Assert.Equal("Subject",          headers.Subject);
        Assert.Equal(["reply@test.com"], headers.ReplyTo);
        Assert.Equal(["cc@test.com"],    headers.Cc);
        Assert.Equal(["bcc@test.com"],   headers.Bcc);
    }
}
