using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

using Amazon.Security;

namespace Amazon.CloudWatch.Tests;

internal static class FakeHttp
{
    // CloudWatchClient has no constructor that takes an HttpClient
    public static CloudWatchClient CreateClient(HttpMessageHandler handler)
    {
        var client = new CloudWatchClient(AwsRegion.USEast1, new AwsCredential("AKIDEXAMPLE", "secret"));

        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
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

    public string? ContentType { get; } = request.Content?.Headers.ContentType?.MediaType;

    public string Body { get; } = body;

    // The decoded form fields, in the order they were sent
    public List<KeyValuePair<string, string>> Form { get; } = ParseForm(body);

    public string? this[string name] => Form.Where(f => f.Key == name).Select(f => f.Value).SingleOrDefault();

    // Selects fields by key suffix, so tests of dimension values don't also depend on the key's prefix
    public string[] ValuesWithKeySuffix(string suffix)
    {
        return Form.Where(f => f.Key.EndsWith(suffix, StringComparison.Ordinal)).Select(f => f.Value).ToArray();
    }

    private static List<KeyValuePair<string, string>> ParseForm(string body)
    {
        var form = new List<KeyValuePair<string, string>>();

        foreach (var pair in body.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = pair.Split('=', 2);

            form.Add(new(Decode(parts[0]), Decode(parts[1])));
        }

        return form;
    }

    private static string Decode(string text) => Uri.UnescapeDataString(text.Replace('+', ' '));
}

internal sealed class FakeHandler(Func<HttpResponseMessage> response) : HttpMessageHandler
{
    public List<RecordedRequest> Requests { get; } = [];

    public RecordedRequest Request => Assert.Single(Requests);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(new RecordedRequest(request, await request.Content!.ReadAsStringAsync(cancellationToken)));

        return response();
    }
}
