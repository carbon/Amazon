using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class SetQueueAttributesRequest : SqsRequest
{
    public SetQueueAttributesRequest(string queueUrl, QueueAttributes attributes)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentNullException.ThrowIfNull(attributes);

        QueueUrl = queueUrl;
        Attributes = attributes;
    }

    [JsonPropertyName("Attributes")]
    public QueueAttributes Attributes { get; }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }
}
