using System.Net.Mail;
using System.Text;
using System.Text.Json;

using Amazon.Ses.Tests;

using MimeKit;

namespace Amazon.Ses.Extensions.Tests;

public class RawMessageExtensionsTests
{
    private static MimeMessage CreateMimeMessage()
    {
        var message = new MimeMessage {
            Subject = "Subject",
            Body    = new TextPart("plain") { Text = "Body" }
        };

        message.From.Add(MailboxAddress.Parse("from@test.com"));
        message.To.Add(MailboxAddress.Parse("to@test.com"));

        return message;
    }

    [Fact]
    public void MimeMessage_ToRawMessage()
    {
        var text = Encoding.ASCII.GetString(CreateMimeMessage().ToRawMessage().Data);

        Assert.Contains("From: from@test.com", text);
        Assert.Contains("To: to@test.com", text);
        Assert.Contains("Subject: Subject", text);
        Assert.EndsWith("Body", text.TrimEnd());
    }

    [Fact]
    public void MailMessage_ToRawMessage()
    {
        using var message = new MailMessage("from@test.com", "to@test.com", "Subject", "Body");

        var text = Encoding.ASCII.GetString(message.ToRawMessage().Data);

        Assert.Contains("From: from@test.com", text);
        Assert.Contains("Subject: Subject", text);
        Assert.Contains("Body", text);
    }

    [Fact]
    public async Task SendEmailAsync_SendsRawContent()
    {
        var handler = new FakeHandler(() => FakeHttp.Json("""{"MessageId":"abc"}"""));
        var client  = FakeHttp.Use(new SesClient(AwsRegion.USEast1, new AwsCredential("public", "private")), handler);
        var message = CreateMimeMessage();

        var result = await client.SendEmailAsync(message);

        Assert.Equal("abc", result.MessageId);

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);

        var raw = body.RootElement.GetProperty("Content").GetProperty("Raw").GetProperty("Data").GetBytesFromBase64();

        Assert.Contains("Subject: Subject", Encoding.ASCII.GetString(raw));
    }
}
