using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class PurgeQueueRequest : SqsRequest
{
    public PurgeQueueRequest(string queueUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queueUrl);

        QueueUrl = queueUrl;
    }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; set; }
}
