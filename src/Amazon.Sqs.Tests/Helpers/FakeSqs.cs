using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Amazon.Sqs.Tests;

internal sealed class RecordedRequest(HttpRequestMessage request, string body)
{
    public HttpMethod Method { get; } = request.Method;

    public Uri RequestUri { get; } = request.RequestUri!;

    public string Target { get; } = request.Headers.GetValues("x-amz-target").First();

    public string Authorization { get; } = request.Headers.NonValidated["Authorization"].ToString();

    public string ContentType { get; } = request.Content!.Headers.NonValidated["Content-Type"].ToString();

    public string Body { get; } = body;

    public JsonElement Json => JsonDocument.Parse(Body).RootElement;
}

internal sealed class FakeSqsHandler(params Func<HttpResponseMessage>[] responses) : HttpMessageHandler
{
    public List<RecordedRequest> Requests { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = await request.Content!.ReadAsStringAsync(cancellationToken);

        Requests.Add(new RecordedRequest(request, body));

        return responses[Math.Min(Requests.Count, responses.Length) - 1]();
    }
}

// Waits until the caller's token is cancelled, like a long poll that never returns
internal sealed class HangingHandler : HttpMessageHandler
{
    public TaskCompletionSource Started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Started.TrySetResult();

        await Task.Delay(Timeout.Infinite, cancellationToken);

        throw new InvalidOperationException("unreachable");
    }
}

internal static class FakeSqs
{
    public const string QueueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/orders";

    public static readonly AwsCredential Credential = new("public", "private");

    // The client has no constructor that takes an HttpClient
    public static SqsClient UseHandler(SqsClient client, HttpMessageHandler handler)
    {
        typeof(AwsClient)
            .GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(client, new HttpClient(handler));

        return client;
    }

    public static SqsClient CreateClient(HttpMessageHandler handler)
    {
        return UseHandler(new SqsClient(AwsRegion.USEast1, Credential), handler);
    }

    public static SqsQueue CreateQueue(HttpMessageHandler handler)
    {
        return UseQueueHandler(new SqsQueue(AwsRegion.USEast1, "123456789012", "orders", Credential), handler);
    }

    public static SqsQueue<QueueItem> CreateTypedQueue(HttpMessageHandler handler)
    {
        return UseQueueHandler(new SqsQueue<QueueItem>(AwsRegion.USEast1, "123456789012", "orders", Credential), handler);
    }

    // The queues create their own client
    private static T UseQueueHandler<T>(T queue, HttpMessageHandler handler)
        where T : class
    {
        var client = (SqsClient)typeof(T)
            .GetField("_client", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(queue)!;

        UseHandler(client, handler);

        return queue;
    }

    public static Func<HttpResponseMessage> Json(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return Content(json, "application/x-amz-json-1.0", statusCode);
    }

    public static Func<HttpResponseMessage> Content(string text, string contentType, HttpStatusCode statusCode)
    {
        return () => new HttpResponseMessage(statusCode) {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes(text)) {
                Headers = {
                    { "Content-Type", contentType }
                }
            }
        };
    }

    public static readonly Func<HttpResponseMessage> Unavailable = Content("", "text/plain", HttpStatusCode.ServiceUnavailable);

    public static readonly Func<HttpResponseMessage> Empty = Json("{}");

    public static readonly Func<HttpResponseMessage> QueueDoesNotExist = Json(
        """{"__type":"com.amazonaws.sqs#QueueDoesNotExist","message":"The specified queue does not exist."}""",
        HttpStatusCode.BadRequest);

    public static Func<HttpResponseMessage> Messages(params string[] bodies)
    {
        var messages = bodies.Select((body, i) => new {
            MessageId     = $"id-{i + 1}",
            ReceiptHandle = $"handle-{i + 1}",
            Body          = body
        });

        return Json(JsonSerializer.Serialize(new { Messages = messages }));
    }

    public static readonly Func<HttpResponseMessage> BatchWithFailure = Json(
        """
        {
          "Failed": [ { "Id": "2", "Code": "InternalError", "SenderFault": false, "Message": "try again" } ],
          "Successful": []
        }
        """);

    public static SqsMessage Received(string handle, string body = "{}")
    {
        return new SqsMessage { MessageId = "id-" + handle, ReceiptHandle = handle, Body = body };
    }
}

public sealed class QueueItem
{
    public int Position { get; set; }

    public string? Text { get; set; }
}
