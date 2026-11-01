using System.IO;
using System.Net;
using System.Text;

namespace Amazon.S3.Tests;

internal sealed class RecordedRequest
{
    public RecordedRequest(HttpRequestMessage request, byte[] body)
    {
        Method = request.Method;
        Uri    = request.RequestUri!.ToString();
        Body   = body;

        foreach (var header in request.Headers.NonValidated)
        {
            Headers[header.Key] = header.Value.ToString();
        }

        if (request.Content is not null)
        {
            foreach (var header in request.Content.Headers.NonValidated)
            {
                Headers[header.Key] = header.Value.ToString();
            }
        }
    }

    public HttpMethod Method { get; }

    public string Uri { get; }

    public byte[] Body { get; }

    public string BodyText => Encoding.UTF8.GetString(Body);

    public Dictionary<string, string> Headers { get; } = new(StringComparer.OrdinalIgnoreCase);
}

internal sealed class FakeS3Handler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> respond) : HttpMessageHandler
{
    private readonly List<RecordedRequest> _requests = [];

    /// <summary>
    /// Answers each request with the next response, repeating the last one
    /// </summary>
    public static FakeS3Handler Sequence(params Func<HttpResponseMessage>[] responses)
    {
        int index = -1;

        return new FakeS3Handler((_, _) => {
            int i = Interlocked.Increment(ref index);

            return Task.FromResult(responses[Math.Min(i, responses.Length - 1)]());
        });
    }

    public IReadOnlyList<RecordedRequest> Requests
    {
        get
        {
            lock (_requests)
            {
                return [.. _requests];
            }
        }
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        byte[] body = request.Content is null ? [] : await request.Content.ReadAsByteArrayAsync(cancellationToken);

        lock (_requests)
        {
            _requests.Add(new RecordedRequest(request, body));
        }

        var response = await respond(request, cancellationToken);

        response.RequestMessage = request;

        return response;
    }
}

internal static class FakeS3
{
    public static S3Client CreateClient(FakeS3Handler handler)
    {
        return new S3Client(AwsRegion.USEast1, "s3.test", new AwsCredential("public", "private"), new HttpClient(handler));
    }

    public static S3Bucket CreateBucket(FakeS3Handler handler)
    {
        return new S3Bucket("bucket", CreateClient(handler));
    }

    public static HttpResponseMessage Status(HttpStatusCode statusCode)
    {
        return new HttpResponseMessage(statusCode);
    }

    public static HttpResponseMessage Xml(string xml, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode) {
            Content = new StringContent(xml, Encoding.UTF8, "application/xml")
        };
    }

    public static HttpResponseMessage Error(string code, HttpStatusCode statusCode)
    {
        return Xml($"<Error><Code>{code}</Code><Message>{code} message</Message></Error>", statusCode);
    }

    public static HttpResponseMessage Unavailable() => Error("SlowDown", HttpStatusCode.ServiceUnavailable);

    public static HttpResponseMessage BrokenPipe()
    {
        throw new HttpRequestException("Broken pipe", new IOException("The response ended prematurely"));
    }

    public static HttpResponseMessage WithETag(string eTag)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        response.Headers.TryAddWithoutValidation("ETag", eTag);

        return response;
    }

    public static HttpResponseMessage Blob(byte[] data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode) {
            Content = new ByteArrayContent(data) {
                Headers = {
                    { "Content-Type", "application/octet-stream" },
                    { "Content-Length", data.Length.ToString(System.Globalization.CultureInfo.InvariantCulture) },
                    { "Last-Modified", "Wed, 01 Jan 2020 01:01:01 GMT" }
                }
            }
        };
    }

    public static HttpResponseMessage Head(long contentLength)
    {
        return new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new ByteArrayContent([]) {
                Headers = {
                    { "Content-Type", "application/octet-stream" },
                    { "Content-Length", contentLength.ToString(System.Globalization.CultureInfo.InvariantCulture) },
                    { "Last-Modified", "Wed, 01 Jan 2020 01:01:01 GMT" }
                }
            }
        };
    }
}
