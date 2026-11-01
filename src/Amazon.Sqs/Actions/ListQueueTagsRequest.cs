using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ListQueueTagsRequest : SqsRequest
{
    public ListQueueTagsRequest(string queueUrl)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);

        QueueUrl = queueUrl;
    }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }
}
