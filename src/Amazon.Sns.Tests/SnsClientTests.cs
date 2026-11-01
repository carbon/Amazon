using System.Net;
using System.Net.Http;
using System.Text;

namespace Amazon.Sns.Tests;

public class SnsClientTests
{
    private const string TopicArn = "arn:aws:sns:us-east-1:123456789012:MyTopic";

    private static SnsClient CreateClient(FakeHandler handler)
    {
        return FakeSns.Use(new SnsClient(AwsRegion.USEast1, new AwsCredential("public", "private")), handler);
    }

    private static FakeHandler Success() => new(() => FakeSns.Xml(FakeSns.PublishResponseXml));

    [Fact]
    public void Constructor_RejectsNullArguments()
    {
        Assert.Throws<ArgumentNullException>(() => new SnsClient(null!, new AwsCredential("public", "private")));
        Assert.Throws<ArgumentNullException>(() => new SnsClient(AwsRegion.USEast1, null!));
    }

    [Fact]
    public void Endpoint_IncludesRegion()
    {
        var client = new SnsClient(AwsRegion.USWest2, new AwsCredential("public", "private"));

        Assert.Equal("https://sns.us-west-2.amazonaws.com/", client.Endpoint);
        Assert.Equal(AwsRegion.USWest2, client.Region);
    }

    [Fact]
    public async Task PublishAsync_PostsSignedForm()
    {
        var handler = Success();

        await CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello world!"));

        var request = Assert.Single(handler.Requests);

        Assert.Same(HttpMethod.Post, request.Method);
        Assert.Equal("https://sns.us-east-1.amazonaws.com/", request.RequestUri.ToString());
        Assert.StartsWith("application/x-www-form-urlencoded", request.ContentType);
        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", request.Authorization);
        Assert.Contains("/us-east-1/sns/aws4_request", request.Authorization);

        Assert.Equal(
        [
            new("Action", "Publish"),
            new("TopicArn", TopicArn),
            new("Message", "Hello world!"),
            new("Version", "2010-03-31")
        ], request.Form);
    }

    [Fact]
    public async Task PublishAsync_IncludesSubject()
    {
        var handler = Success();

        await CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello", subject: "My first message"));

        Assert.Equal(
        [
            new("Action", "Publish"),
            new("TopicArn", TopicArn),
            new("Message", "Hello"),
            new("Subject", "My first message"),
            new("Version", "2010-03-31")
        ], handler.Requests[0].Form);
    }

    [Theory]
    [InlineData("a&b=c+d e%20f")]
    [InlineData("line1\nline2")]
    [InlineData("""{"default":"text"}""")]
    [InlineData("héllo wörld ✓ 🎉")]
    public async Task PublishAsync_EncodesMessage(string message)
    {
        var handler = Success();

        await CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, message));

        Assert.Single(handler.Requests[0].Form, p => p.Key == "Message" && p.Value == message);
    }

    [Fact]
    public async Task PublishAsync_ReturnsResponseBody()
    {
        var result = await CreateClient(Success()).PublishAsync(new PublishRequest(TopicArn, "Hello"));

        Assert.Equal(FakeSns.PublishResponseXml, Encoding.UTF8.GetString(result));
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task PublishAsync_ThrowsWithResponseText_WithoutRetrying(HttpStatusCode statusCode)
    {
        var handler = new FakeHandler(() => FakeSns.Xml(FakeSns.NotFoundErrorXml, statusCode));

        var ex = await Assert.ThrowsAsync<Exception>(
            () => CreateClient(handler).PublishAsync(new PublishRequest(TopicArn, "Hello")));

        Assert.Equal(FakeSns.NotFoundErrorXml, ex.Message);
        Assert.Single(handler.Requests);
    }
}
