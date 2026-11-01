using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Tests;

internal sealed class RecordedRequest(HttpRequestMessage request, string body)
{
    public string Target { get; } = request.Headers.GetValues("x-amz-target").First();

    public string Authorization { get; } = request.Headers.NonValidated["Authorization"].ToString();

    public string ContentType { get; } = request.Content!.Headers.NonValidated["Content-Type"].ToString();

    public Uri Uri { get; } = request.RequestUri!;

    public string Body { get; } = body;
}

internal sealed class FakeDynamoHandler(params Func<HttpResponseMessage>[] responses) : HttpMessageHandler
{
    public List<RecordedRequest> Requests { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = await request.Content!.ReadAsStringAsync(cancellationToken);

        Requests.Add(new RecordedRequest(request, body));

        return responses[Math.Min(Requests.Count, responses.Length) - 1]();
    }
}

internal static class FakeDynamo
{
    public static readonly AwsCredential Credential = new("public", "private");

    // The clients have no constructor that takes an HttpClient
    public static T Use<T>(T client, HttpMessageHandler handler)
        where T : AwsClient
    {
        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
    }

    public static DynamoDbClient CreateClient(FakeDynamoHandler handler)
    {
        return Use(new DynamoDbClient(AwsRegion.USEast1, Credential), handler);
    }

    public static DynamoTable<Widget, long> CreateTable(FakeDynamoHandler handler)
    {
        return new DynamoTable<Widget, long>("Widgets", CreateClient(handler));
    }

    public static HttpResponseMessage Json(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode) {
            Content = new StringContent(json, Encoding.UTF8, "application/x-amz-json-1.0")
        };
    }

    public static HttpResponseMessage Error(string type, string message = "Failed", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return Json($$"""{"__type":"com.amazonaws.dynamodb.v20120810#{{type}}","message":"{{message}}"}""", statusCode);
    }

    public static HttpResponseMessage Throttled() => Error("ProvisionedThroughputExceededException");
}

[Dataset("Widgets")]
public class Widget
{
    [Member("id"), Key]
    public long Id { get; set; }

    [Member("name")]
    public string Name { get; set; }
}
