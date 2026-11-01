using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Amazon.Elb.Tests;

internal static class FakeElb
{
    // ElbClient has no constructor that takes an HttpClient
    public static ElbClient Create(FakeElbHandler handler)
    {
        var client = new ElbClient(AwsRegion.USEast1, new AwsCredential("public", "private"));

        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
    }

    public static string EmptyResponse(string rootName)
    {
        return $"""<{rootName} xmlns="{ElbClient.Namespace}"><ResponseMetadata><RequestId>x</RequestId></ResponseMetadata></{rootName}>""";
    }
}

internal sealed class RecordedElbRequest(HttpRequestMessage request, string body)
{
    public HttpMethod Method { get; } = request.Method;

    public Uri RequestUri { get; } = request.RequestUri!;

    public string? Authorization { get; } = request.Headers.TryGetValues("Authorization", out var values) ? values.First() : null;

    public string? ContentType { get; } = request.Content?.Headers.ContentType?.MediaType;

    public string Body { get; } = body;

    public List<KeyValuePair<string, string>> Parameters { get; } = body.Split('&')
        .Select(static pair => pair.Split('='))
        .Select(static pair => new KeyValuePair<string, string>(WebUtility.UrlDecode(pair[0]), WebUtility.UrlDecode(pair[1])))
        .ToList();

    public string this[string name] => Parameters.Single(p => p.Key == name).Value;
}

internal sealed class FakeElbHandler(string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK) : HttpMessageHandler
{
    public List<RecordedElbRequest> Requests { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = await request.Content!.ReadAsStringAsync(cancellationToken);

        Requests.Add(new RecordedElbRequest(request, body));

        return new HttpResponseMessage(statusCode) {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes(responseBody))
        };
    }
}
