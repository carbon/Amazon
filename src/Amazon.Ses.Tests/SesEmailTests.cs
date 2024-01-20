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

        var content = string.Join('&', sesEmail.ToParameters().Select(p => $"{p.Key}={p.Value}"));

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

        var content = string.Join('&', sesEmail.ToParameters().Select(p => $"{p.Key}={p.Value}"));

        Assert.Equal("Source=from@test.com&Message.Subject.Data=Subject&Message.Subject.Charset=UTF-8&Message.Body.Html.Data=Body&Message.Body.Html.Charset=UTF-8&ReplyToAddresses.member.1=c@test.com&Destination.ToAddresses.member.1=to@test.com&Destination.CcAddresses.member.1=a@test.com&Destination.CcAddresses.member.2=b@test.com", content);

        Assert.Equal(9, q.Count);
    }
}