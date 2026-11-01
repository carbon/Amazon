using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Amazon.Ses.Tests;

internal static class FakeHttp
{
    // The service clients have no constructor that takes an HttpClient
    public static T Use<T>(T client, HttpMessageHandler handler)
        where T : AwsClient
    {
        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
    }

    public static HttpResponseMessage Json(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return Content(json, "application/json", statusCode);
    }

    public static HttpResponseMessage Content(string text, string contentType, HttpStatusCode statusCode)
    {
        return new HttpResponseMessage(statusCode) {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes(text)) {
                Headers = {
                    { "Content-Type", contentType }
                }
            }
        };
    }
}

internal sealed class RecordedRequest(HttpRequestMessage request, string? body)
{
    public HttpMethod Method { get; } = request.Method;

    public Uri RequestUri { get; } = request.RequestUri!;

    public string Target { get; } = request.Headers.GetValues("x-amz-target").First();

    public string? Authorization { get; } = request.Headers.TryGetValues("Authorization", out var values) ? values.First() : null;

    public string? ContentType { get; } = request.Content?.Headers.ContentType?.ToString();

    public string? Body { get; } = body;
}

internal sealed class FakeHandler(params Func<HttpResponseMessage>[] responses) : HttpMessageHandler
{
    public List<RecordedRequest> Requests { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);

        Requests.Add(new RecordedRequest(request, body));

        return responses[Math.Min(Requests.Count, responses.Length) - 1]();
    }
}
