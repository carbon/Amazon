using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ChangeMessageVisibilityBatchRequest : SqsRequest
{
    public ChangeMessageVisibilityBatchRequest(string queueUrl, ChangeMessageVisibilityBatchRequestEntry[] entries)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentNullException.ThrowIfNull(entries);

        QueueUrl = queueUrl;
        Entries = entries;
    }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }

    [JsonPropertyName("Entries")]
    public ChangeMessageVisibilityBatchRequestEntry[] Entries { get; }
}