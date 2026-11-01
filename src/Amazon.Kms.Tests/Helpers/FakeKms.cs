using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Amazon.Kms.Tests;

internal static class FakeKms
{
    // KmsClient has no constructor that takes an HttpClient
    public static KmsClient CreateClient(FakeKmsHandler handler)
    {
        var client = new KmsClient(AwsRegion.USEast1, new AwsCredential("public", "private"));

        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
    }

    public static HttpResponseMessage Response(string text, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode) {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes(text)) {
                Headers = {
                    { "Content-Type", "application/x-amz-json-1.1" }
                }
            }
        };
    }
}

internal sealed class RecordedKmsRequest(HttpRequestMessage request, string body)
{
    public HttpMethod Method { get; } = request.Method;

    public Uri RequestUri { get; } = request.RequestUri!;

    public string Target { get; } = request.Headers.GetValues("x-amz-target").Single();

    public string Authorization { get; } = request.Headers.GetValues("Authorization").Single();

    public string? ContentType { get; } = request.Content!.Headers.ContentType?.ToString();

    public string Body { get; } = body;

    public JsonElement Json { get; } = JsonDocument.Parse(body).RootElement;
}

internal sealed class FakeKmsHandler(string responseText = "{}", HttpStatusCode statusCode = HttpStatusCode.OK) : HttpMessageHandler
{
    public List<RecordedKmsRequest> Requests { get; } = [];

    public RecordedKmsRequest Request => Requests.Single();

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(new RecordedKmsRequest(request, await request.Content!.ReadAsStringAsync(cancellationToken)));

        return FakeKms.Response(responseText, statusCode);
    }
}
