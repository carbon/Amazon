using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class RemovePermissionRequest : SqsRequest
{
    public RemovePermissionRequest(string queueUrl, string label)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentException.ThrowIfNullOrEmpty(label);

        QueueUrl = queueUrl;
        Label = label;
    }

    [JsonPropertyName("Label")]
    public string Label { get; }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }
}
