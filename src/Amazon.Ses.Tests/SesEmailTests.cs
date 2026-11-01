using System.Net.Mail;

namespace Amazon.Ses.Tests;

public class SesEmailTests
{
    [Fact]
    public void CanConstructFromMailMessage()
    {
        var message = new MailMessage("from@test.com", "to@test.com", "Subject", "Body");

        var sesEmail = SesEmail.FromMailMessage(message);

        Assert.Equal("from@test.com",  sesEmail.Source);
        Assert.Equal(["to@test.com"],  sesEmail.To.AsSpan());
        Assert.Equal("Body",           sesEmail.Text?.Data);
        Assert.Equal("Subject",        sesEmail.Subject.Data);

        var content = string.Join('&', sesEmail.ToParameters().Select(static p => $"{p.Key}={p.Value}"));

        Assert.Equal("Source=from@test.com&Message.Subject.Data=Subject&Message.Subject.Charset=UTF-8&Message.Body.Text.Data=Body&Message.Body.Text.Charset=UTF-8&Destination.ToAddresses.member.1=to@test.com", content);
    }

    [Fact]
    public void CanConstructFromMailMessage_Complex()
    {
        var message = new MailMessage("from@test.com", "to@test.com", "Subject", "Body") {
            IsBodyHtml = true,
            CC = {
                "a@test.com",
                "b@test.com"
            },
            ReplyToList = {
                "c@test.com"
            }
        };

        var sesEmail = SesEmail.FromMailMessage(message);

        Assert.Equal("from@test.com", sesEmail.Source);
        Assert.Equal(["to@test.com"], sesEmail.To.AsSpan());
        Assert.Equal(["a@test.com", "b@test.com"], sesEmail.Cc.AsSpan());
        Assert.Equal(["c@test.com"], sesEmail.ReplyTo.AsSpan());
        Assert.Null(sesEmail.Bcc);

        Assert.Equal("Body", sesEmail.Html?.Data);
        Assert.Equal("Subject", sesEmail.Subject.Data);

        var q = sesEmail.ToParameters();

        var content = string.Join('&', sesEmail.ToParameters().Select(static p => $"{p.Key}={p.Value}"));

        Assert.Equal("Source=from@test.com&Message.Subject.Data=Subject&Message.Subject.Charset=UTF-8&Message.Body.Html.Data=Body&Message.Body.Html.Charset=UTF-8&ReplyToAddresses.member.1=c@test.com&Destination.ToAddresses.member.1=to@test.com&Destination.CcAddresses.member.1=a@test.com&Destination.CcAddresses.member.2=b@test.com", content);

        Assert.Equal(9, q.Count);
    }

    [Fact]
    public void FromMailMessage_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => SesEmail.FromMailMessage(null));
    }

    [Fact]
    public void FromMailMessage_EncodesDisplayNames()
    {
        var message = new MailMessage(new MailAddress("from@test.com", "Sender"), new MailAddress("to@test.com", "☻")) {
            Subject = "Subject",
            Body    = "Body"
        };

        var sesEmail = SesEmail.FromMailMessage(message);

        Assert.Equal("\"Sender\" <from@test.com>", sesEmail.Source);
        Assert.Equal(["\"=?utf-8?Q?=E2=98=BB?=\" <to@test.com>"], sesEmail.To.AsSpan());
    }

    [Fact]
    public void FromMailMessage_UsesObsoleteReplyTo()
    {
        var message = new MailMessage("from@test.com", "to@test.com", "Subject", "Body");

#pragma warning disable CS0618 // Type or member is obsolete
        message.ReplyTo = new MailAddress("reply@test.com");
#pragma warning restore CS0618

        Assert.Equal(["reply@test.com"], SesEmail.FromMailMessage(message).ReplyTo.AsSpan());
    }

    [Fact]
    public void FromMailMessage_KeepsCcAndBccSeparate()
    {
        var message = new MailMessage("from@test.com", "to@test.com", "Subject", "Body") {
            CC  = { "cc@test.com" },
            Bcc = { "bcc@test.com" }
        };

        var sesEmail = SesEmail.FromMailMessage(message);

        Assert.Equal(["cc@test.com"],  sesEmail.Cc.AsSpan());
        Assert.Equal(["bcc@test.com"], sesEmail.Bcc.AsSpan());
    }

    [Fact]
    public void FromMailMessage_ReadsAlternateViews()
    {
        var message = new MailMessage("from@test.com", "to@test.com", "Subject", "Body");

        message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("<b>Html</b>", null, "text/html"));
        message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("Text", null, "text/plain"));
        message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("BEGIN:VCALENDAR", null, "text/calendar"));

        var sesEmail = SesEmail.FromMailMessage(message);

        Assert.Equal("<b>Html</b>", sesEmail.Html?.Data);
        Assert.Equal("Text",        sesEmail.Text?.Data);
    }

    [Fact]
    public void ToParameters_IncludesBccAndOmitsDefaultCharset()
    {
        var sesEmail = new SesEmail {
            Source  = "from@test.com",
            To      = ["to@test.com"],
            Bcc     = ["a@test.com", "b@test.com"],
            Subject = new SesContent("Subject")
        };

        var content = string.Join('&', sesEmail.ToParameters().Select(static p => $"{p.Key}={p.Value}"));

        Assert.Equal("Source=from@test.com&Message.Subject.Data=Subject&Destination.ToAddresses.member.1=to@test.com&Destination.BccAddresses.member.1=a@test.com&Destination.BccAddresses.member.2=b@test.com", content);
    }
}