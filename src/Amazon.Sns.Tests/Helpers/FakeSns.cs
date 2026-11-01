using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Amazon.Sns.Tests;

internal static class FakeSns
{
    public const string PublishResponseXml =
        """
        <PublishResponse xmlns="https://sns.amazonaws.com/doc/2010-03-31/">
          <PublishResult>
            <MessageId>94f20ce6-13c5-43a0-9a9e-ca52d816e90b</MessageId>
          </PublishResult>
          <ResponseMetadata>
            <RequestId>f187a3c1-376f-11df-8963-01868b7c937a</RequestId>
          </ResponseMetadata>
        </PublishResponse>
        """;

    public const string NotFoundErrorXml =
        """
        <ErrorResponse xmlns="https://sns.amazonaws.com/doc/2010-03-31/">
          <Error>
            <Type>Sender</Type>
            <Code>NotFound</Code>
            <Message>Topic does not exist</Message>
          </Error>
          <RequestId>f187a3c1-376f-11df-8963-01868b7c937a</RequestId>
        </ErrorResponse>
        """;

    // SnsClient has no constructor that takes an HttpClient
    public static SnsClient Use(SnsClient client, HttpMessageHandler handler)
    {
        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
    }

    // SnsTopic creates its own SnsClient
    public static SnsTopic Use(SnsTopic topic, HttpMessageHandler handler)
    {
        var client = (SnsClient)typeof(SnsTopic)
            .GetField("_client", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(topic)!;

        Use(client, handler);

        return topic;
    }

    public static HttpResponseMessage Xml(string xml, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode) {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes(xml)) {
                Headers = {
                    { "Content-Type", "text/xml" }
                }
            }
        };
    }
}

internal sealed class RecordedRequest(HttpRequestMessage request, string body)
{
    public HttpMethod Method { get; } = request.Method;

    public Uri RequestUri { get; } = request.RequestUri!;

    public string? Authorization { get; } = request.Headers.TryGetValues("Authorization", out var values) ? values.First() : null;

    public string? ContentType { get; } = request.Content?.Headers.ContentType?.ToString();

    public string Body { get; } = body;

    // The form fields, decoded and in the order they were sent
    public List<KeyValuePair<string, string>> Form { get; } = ParseForm(body);

    private static List<KeyValuePair<string, string>> ParseForm(string body)
    {
        var result = new List<KeyValuePair<string, string>>();

        foreach (var pair in body.Split('&'))
        {
            var parts = pair.Split('=', 2);

            result.Add(new(Decode(parts[0]), Decode(parts[1])));
        }

        return result;
    }

    private static string Decode(string value) => Uri.UnescapeDataString(value.Replace('+', ' '));
}

internal sealed class FakeHandler(params Func<HttpResponseMessage>[] responses) : HttpMessageHandler
{
    public List<RecordedRequest> Requests { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var body = await request.Content!.ReadAsStringAsync(cancellationToken);

        Requests.Add(new RecordedRequest(request, body));

        return responses[Math.Min(Requests.Count, responses.Length) - 1]();
    }
}
