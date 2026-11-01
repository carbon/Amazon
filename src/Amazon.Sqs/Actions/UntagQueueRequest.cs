using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class UntagQueueRequest : SqsRequest
{
    public UntagQueueRequest(string queueUrl, string[] tagKeys)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentNullException.ThrowIfNull(tagKeys);

        QueueUrl = queueUrl;
        TagKeys = tagKeys;
    }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }

    [JsonPropertyName("TagKeys")]
    public string[] TagKeys { get; }
}
