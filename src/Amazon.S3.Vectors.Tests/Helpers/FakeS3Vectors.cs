using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Amazon.S3.Vectors.Tests;

internal static class FakeS3Vectors
{
    public const string BucketArn = "arn:aws:s3vectors:us-east-1:123456789012:bucket/media";

    public const string EncodedBucketArn = "arn%3Aaws%3As3vectors%3Aus-east-1%3A123456789012%3Abucket%2Fmedia";

    public static AmazonS3VectorsClient Client(FakeHandler handler, string region = "us-east-1")
    {
        var client = new AmazonS3VectorsClient(new AwsRegion(region), new AwsCredential("AKIAIOSFODNN7EXAMPLE", "secret"));

        // The client has no constructor that takes an HttpClient
        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
    }

    public static Func<HttpResponseMessage> Json(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return () => new HttpResponseMessage(statusCode) {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    // The void operations answer with a 200 and an empty HTTP body
    public static Func<HttpResponseMessage> Empty()
    {
        return () => new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new ByteArrayContent([])
        };
    }
}

internal sealed class RecordedRequest(HttpRequestMessage request, string? body)
{
    public HttpMethod Method { get; } = request.Method;

    public Uri RequestUri { get; } = request.RequestUri!;

    public string? Authorization { get; } = request.Headers.TryGetValues("Authorization", out var values) ? values.First() : null;

    public string? Accept { get; } = request.Headers.Accept.ToString();

    public string? MediaType { get; } = request.Content?.Headers.ContentType?.MediaType;

    public bool HasAmzDate { get; } = request.Headers.Contains("x-amz-date");

    public string? AmzDate { get; } = request.Headers.TryGetValues("x-amz-date", out var dates) ? dates.First() : null;

    public string? Body { get; } = body;

    public JsonElement Json => JsonDocument.Parse(Body!).RootElement;

    public bool IsAbsent(string name) => !Json.TryGetProperty(name, out _);
}

internal sealed class FakeHandler(params Func<HttpResponseMessage>[] responses) : HttpMessageHandler
{
    public List<RecordedRequest> Requests { get; } = [];

    public RecordedRequest Request => Assert.Single(Requests);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);

        Requests.Add(new RecordedRequest(request, body));

        return responses[Math.Min(Requests.Count, responses.Length) - 1]();
    }
}
