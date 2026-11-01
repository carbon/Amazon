using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.Json;

namespace Amazon.Ses.Tests;

public class SesClientTests
{
    private const string SendEmailResultJson = """{"MessageId":"abc"}""";

    private static SesClient CreateClient(FakeHandler handler)
    {
        return FakeHttp.Use(new SesClient(AwsRegion.USEast1, new AwsCredential("public", "private")), handler);
    }

    [Fact]
    public async Task SendEmailAsync_PostsSignedRequest()
    {
        var handler = new FakeHandler(() => FakeHttp.Json(SendEmailResultJson));

        var result = await CreateClient(handler).SendEmailAsync(new SendEmailRequest {
            Content = new RawMessage([1, 2, 3])
        });

        Assert.Equal("abc", result.MessageId);

        var request = Assert.Single(handler.Requests);

        Assert.Same(HttpMethod.Post, request.Method);
        Assert.Equal("https://email.us-east-1.amazonaws.com/v2/email/outbound-emails", request.RequestUri.ToString());
        Assert.Equal("SESv2.SendEmail", request.Target);
        Assert.Equal("application/json", request.ContentType);
        Assert.Equal("""{"Content":{"Raw":{"Data":"AQID"}}}""", request.Body);
        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", request.Authorization);
        Assert.Contains("/us-east-1/ses/aws4_request", request.Authorization);
    }

    [Fact]
    public async Task SendEmailAsync_SesEmail()
    {
        var handler = new FakeHandler(() => FakeHttp.Json(SendEmailResultJson));

        await CreateClient(handler).SendEmailAsync(new SesEmail {
            Source  = "from@test.com",
            To      = ["to@test.com"],
            Subject = new SesContent("Subject")
        });

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);

        Assert.Equal("from@test.com", body.RootElement.GetProperty("FromEmailAddress").GetString());
        Assert.Equal("to@test.com",   body.RootElement.GetProperty("Destination").GetProperty("ToAddresses")[0].GetString());
    }

    [Fact]
    public async Task SendEmailAsync_MailMessage()
    {
        var handler = new FakeHandler(() => FakeHttp.Json(SendEmailResultJson));

        await CreateClient(handler).SendEmailAsync(new MailMessage("from@test.com", "to@test.com", "Subject", "Body"));

        using var body = JsonDocument.Parse(handler.Requests[0].Body!);

        var simple = body.RootElement.GetProperty("Content").GetProperty("Simple");

        Assert.Equal("Subject", simple.GetProperty("Subject").GetProperty("Data").GetString());
        Assert.Equal("Body",    simple.GetProperty("Body").GetProperty("Text").GetProperty("Data").GetString());
    }

    [Fact]
    public async Task GetAccountAsync()
    {
        var handler = new FakeHandler(() => FakeHttp.Json("""{"SendQuota":{"Max24HourSend":200.0,"MaxSendRate":1.0,"SentLast24Hours":127.0}}"""));

        var result = await CreateClient(handler).GetAccountAsync();

        Assert.Equal(200, result.SendQuota.Max24HourSend);
        Assert.Equal(1,   result.SendQuota.MaxSendRate);
        Assert.Equal(127, result.SendQuota.SentLast24Hours);

        var request = Assert.Single(handler.Requests);

        Assert.Same(HttpMethod.Get, request.Method);
        Assert.Equal("https://email.us-east-1.amazonaws.com/v2/email/account", request.RequestUri.ToString());
        Assert.Equal("SESv2.GetAccount", request.Target);
    }

    [Fact]
    public async Task JsonError_ThrowsSesExceptionWithMessage()
    {
        var handler = new FakeHandler(() => FakeHttp.Json("""{"message":"Email address is not verified."}""", HttpStatusCode.BadRequest));

        var ex = await Assert.ThrowsAsync<SesException>(() => CreateClient(handler).GetAccountAsync());

        Assert.Equal("Email address is not verified.", ex.Message);
        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
    }

    [Fact]
    public async Task JsonError_WithoutMessage_UsesFallback()
    {
        var handler = new FakeHandler(() => FakeHttp.Json("""{"message":null}""", HttpStatusCode.BadRequest));

        var ex = await Assert.ThrowsAsync<SesException>(() => CreateClient(handler).GetAccountAsync());

        Assert.Equal("Something went wrong", ex.Message);
    }

    [Fact]
    public async Task NonJsonError_UsesResponseText()
    {
        var handler = new FakeHandler(() => FakeHttp.Content("Forbidden", "text/plain", HttpStatusCode.Forbidden));

        var ex = await Assert.ThrowsAsync<SesException>(() => CreateClient(handler).GetAccountAsync());

        Assert.Equal("Forbidden", ex.Message);
        Assert.Equal(HttpStatusCode.Forbidden, ex.HttpStatusCode);
    }

    [Fact]
    public async Task ErrorWithoutContentType_ThrowsSesException()
    {
        var handler = new FakeHandler(() => new HttpResponseMessage(HttpStatusCode.BadRequest) {
            Content = new ByteArrayContent([])
        });

        await Assert.ThrowsAsync<SesException>(() => CreateClient(handler).GetAccountAsync());
    }

    [Fact]
    public async Task JsonError_WithCharset_ThrowsSesExceptionWithMessage()
    {
        var handler = new FakeHandler(() => FakeHttp.Content("""{"message":"Rejected"}""", "application/json; charset=utf-8", HttpStatusCode.BadRequest));

        var ex = await Assert.ThrowsAsync<SesException>(() => CreateClient(handler).GetAccountAsync());

        Assert.Equal("Rejected", ex.Message);
    }

    [Fact]
    public async Task SendEmailAsync_DoesNotRetryPermanentError()
    {
        var handler = new FakeHandler(() => FakeHttp.Json("""{"message":"Rejected"}""", HttpStatusCode.BadRequest));

        var request = new SendEmailRequest { Content = new RawMessage([1]) };

        await Assert.ThrowsAsync<SesException>(() => CreateClient(handler).SendEmailAsync(request));

        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task SendEmailAsync_RetriesTransientError()
    {
        var handler = new FakeHandler(
            () => FakeHttp.Json("""{"message":"Try again"}""", HttpStatusCode.ServiceUnavailable),
            () => FakeHttp.Json(SendEmailResultJson));

        var result = await CreateClient(handler).SendEmailAsync(new SendEmailRequest {
            Content = new RawMessage([1, 2, 3])
        });

        Assert.Equal("abc", result.MessageId);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(handler.Requests[0].Body, handler.Requests[1].Body);
    }
}
