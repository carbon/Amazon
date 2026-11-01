using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Amazon.Ec2.Tests;

internal static class FakeEc2
{
    // Ec2Client has no constructor that takes an HttpClient
    public static Ec2Client Client(HttpMessageHandler handler)
    {
        var client = new Ec2Client(AwsRegion.USEast1, new AwsCredential("public", "private"));

        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
    }

    public static Func<HttpResponseMessage> Xml(string xml, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return () => new HttpResponseMessage(statusCode) {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes(xml))
        };
    }

    public static string Response(string name, string innerXml)
    {
        return $"""<{name} xmlns="{Ec2Client.Namespace}"><requestId>r-1</requestId>{innerXml}</{name}>""";
    }
}

internal sealed class RecordedEc2Request(HttpRequestMessage request, string body)
{
    public HttpMethod Method { get; } = request.Method;

    public Uri RequestUri { get; } = request.RequestUri!;

    public string? Authorization { get; } = request.Headers.TryGetValues("Authorization", out var values) ? values.First() : null;

    public string? ContentType { get; } = request.Content?.Headers.ContentType?.MediaType;

    public string Body { get; } = body;

    public Dictionary<string, string> Form { get; } = body
        .Split('&')
        .Select(static pair => pair.Split('='))
        .ToDictionary(static pair => Decode(pair[0]), static pair => Decode(pair[1]));

    private static string Decode(string value) => Uri.UnescapeDataString(value.Replace('+', ' '));
}

internal sealed class FakeEc2Handler(params Func<HttpResponseMessage>[] responses) : HttpMessageHandler
{
    public List<RecordedEc2Request> Requests { get; } = [];

    public RecordedEc2Request Request => Assert.Single(Requests);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = await request.Content!.ReadAsStringAsync(cancellationToken);

        Requests.Add(new RecordedEc2Request(request, body));

        return responses[Math.Min(Requests.Count, responses.Length) - 1]();
    }
}
