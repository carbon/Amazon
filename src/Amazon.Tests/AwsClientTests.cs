using System.Net;
using System.Text;

namespace Amazon.Tests;

public class AwsClientTests
{
    private sealed class TestClient : AwsClient
    {
        public TestClient(IAwsCredential credential)
            : base(AwsService.Sqs, AwsRegion.USEast1, credential) { }

        public TestClient(IAwsCredential credential, HttpClient httpClient)
            : base(AwsService.Sqs, AwsRegion.USEast1, credential, httpClient) { }

        public Task<byte[]> PostAsync(string body = "body", CancellationToken cancellationToken = default)
        {
            return SendAsync(new HttpRequestMessage(HttpMethod.Post, Endpoint) { Content = new StringContent(body) }, cancellationToken);
        }
    }

    private sealed class RenewingCredential : IAwsCredential
    {
        public string AccessKeyId { get; private set; } = "old";

        public string SecretAccessKey => "secret";

        public string SecurityToken => "session";

        public bool ShouldRenew => AccessKeyId is "old";

        public Task<bool> RenewAsync()
        {
            AccessKeyId = "new";

            return Task.FromResult(true);
        }
    }

    private sealed class Handler(HttpResponseMessage response) : HttpMessageHandler
    {
        public HttpRequestMessage Request { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;

            return Task.FromResult(response);
        }
    }

    private static readonly AwsCredential s_credential = new("public", "private");

    private static HttpResponseMessage Response(string text, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode) { Content = new StringContent(text) };
    }

    [Fact]
    public void ValidatesArguments()
    {
        Assert.Throws<ArgumentNullException>(() => new TestClient(null));
        Assert.Throws<ArgumentNullException>(() => new TestClient(null, new HttpClient()));
    }

    [Fact]
    public void BuildsEndpointFromServiceAndRegion()
    {
        Assert.Equal("https://sqs.us-east-1.amazonaws.com/", new TestClient(s_credential).Endpoint);
        Assert.Equal("https://sqs.us-east-1.amazonaws.com/", new TestClient(s_credential, new HttpClient()).Endpoint);
        Assert.Same(AwsRegion.USEast1, new TestClient(s_credential).Region);
    }

    [Fact]
    public async Task SendAsync_SignsAndReturnsBody()
    {
        var handler = new Handler(Response("result"));

        var result = await new TestClient(s_credential, new HttpClient(handler)).PostAsync();

        Assert.Equal("result", Encoding.UTF8.GetString(result));

        var headers = handler.Request.Headers;

        Assert.Equal("sqs.us-east-1.amazonaws.com", headers.Host);
        Assert.NotNull(headers.Date);
        Assert.Matches("^[0-9]{8}T[0-9]{6}Z$", headers.GetValues("x-amz-date").Single());
        Assert.False(headers.Contains("x-amz-security-token"));

        string authorization = headers.NonValidated["Authorization"].ToString();

        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", authorization);
        Assert.Contains("/us-east-1/sqs/aws4_request,SignedHeaders=", authorization);
        Assert.Contains(",Signature=", authorization);
    }

    [Fact]
    public async Task SendAsync_RenewsCredentialAndSendsSecurityToken()
    {
        var handler = new Handler(Response("result"));

        await new TestClient(new RenewingCredential(), new HttpClient(handler)).PostAsync();

        Assert.Equal("session", handler.Request.Headers.GetValues("x-amz-security-token").Single());
        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=new/", handler.Request.Headers.NonValidated["Authorization"].ToString());
    }

    [Fact]
    public async Task SendAsync_ThrowsResponseTextOnError()
    {
        var handler = new Handler(Response("Bad request", HttpStatusCode.BadRequest));

        var ex = await Assert.ThrowsAsync<Exception>(() => new TestClient(s_credential, new HttpClient(handler)).PostAsync());

        Assert.Equal("Bad request", ex.Message);
    }
}
