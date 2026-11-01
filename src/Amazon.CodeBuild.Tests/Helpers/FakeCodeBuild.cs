using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Amazon.CodeBuild.Tests;

internal sealed class RecordedRequest(HttpRequestMessage request, string body)
{
    public HttpMethod Method { get; } = request.Method;

    public Uri Uri { get; } = request.RequestUri!;

    public string Target { get; } = request.Headers.GetValues("x-amz-target").First();

    public string Authorization { get; } = request.Headers.NonValidated["Authorization"].ToString();

    public string ContentType { get; } = request.Content!.Headers.NonValidated["Content-Type"].ToString();

    public string Body { get; } = body;
}

internal sealed class FakeCodeBuildHandler(params Func<HttpResponseMessage>[] responses) : HttpMessageHandler
{
    public List<RecordedRequest> Requests { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = await request.Content!.ReadAsStringAsync(cancellationToken);

        Requests.Add(new RecordedRequest(request, body));

        return responses[Math.Min(Requests.Count, responses.Length) - 1]();
    }
}

internal static class FakeCodeBuild
{
    // The client has no constructor that takes an HttpClient
    public static CodeBuildClient CreateClient(HttpMessageHandler handler)
    {
        var client = new CodeBuildClient(AwsRegion.USEast1, new AwsCredential("public", "private"));

        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
    }

    public static HttpResponseMessage Json(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode) {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes(json)) {
                Headers = {
                    { "Content-Type", "application/x-amz-json-1.1" }
                }
            }
        };
    }

    public static HttpResponseMessage Error(string type, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return Json($$"""{"__type":"{{type}}","message":"{{message}}"}""", statusCode);
    }
}
