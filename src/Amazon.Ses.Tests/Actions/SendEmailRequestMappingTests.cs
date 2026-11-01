using System.Text.Json;

namespace Amazon.Ses.Actions.Tests;

public class SendEmailRequestMappingTests
{
    [Fact]
    public void FromSesEmail_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => SendEmailRequest.FromSesEmail(null));
    }

    [Fact]
    public void FromSesEmail_MapsAllFields()
    {
        var request = SendEmailRequest.FromSesEmail(new SesEmail {
            Source  = "from@test.com",
            To      = ["to@test.com"],
            Cc      = ["cc@test.com"],
            Bcc     = ["bcc@test.com"],
            ReplyTo = ["reply@test.com"],
            Subject = new SesContent("Subject", CharsetType.UTF8),
            Html    = new SesContent("Html", CharsetType.UTF8),
            Text    = new SesContent("Text", CharsetType.UTF8)
        });

        Assert.Equal(
            """
            {"Content":{"Simple":{"Body":{"Html":{"Charset":"UTF-8","Data":"Html"},"Text":{"Charset":"UTF-8","Data":"Text"}},"Subject":{"Charset":"UTF-8","Data":"Subject"}}},"Destination":{"BccAddresses":["bcc@test.com"],"CcAddresses":["cc@test.com"],"ToAddresses":["to@test.com"]},"FromEmailAddress":"from@test.com","ReplyToAddresses":["reply@test.com"]}
            """, JsonSerializer.Serialize(request));
    }

    [Fact]
    public void FromSesEmail_OmitsMissingBodies()
    {
        var request = SendEmailRequest.FromSesEmail(new SesEmail {
            Source  = "from@test.com",
            To      = ["to@test.com"],
            Subject = new SesContent("Subject"),
            Text    = new SesContent("Text")
        });

        var body = request.Content.Simple!.Body;

        Assert.Null(body.Html);
        Assert.Equal("Text", body.Text?.Data);
    }

    [Fact]
    public void RawMessage_SerializesAsBase64()
    {
        EmailContent content = new RawMessage([1, 2, 3]);

        Assert.Null(content.Simple);
        Assert.Equal("""{"Raw":{"Data":"AQID"}}""", JsonSerializer.Serialize(content));
    }

    [Fact]
    public void RawMessage_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RawMessage(null));
    }

    [Fact]
    public void MessageHeader_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new MessageHeader(null, "value"));
        Assert.Throws<ArgumentNullException>(() => new MessageHeader("name", null));
    }

    [Fact]
    public void Content_ImplicitConversionUsesUtf8()
    {
        Content content = "Hello";

        Assert.Equal("Hello", content.Data);
        Assert.Equal("UTF-8", content.Charset);
    }
}
