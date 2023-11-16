using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class DeleteQueueRequest(string queueUrl) : SqsRequest
{
    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; } = queueUrl ?? throw new ArgumentNullException(nameof(queueUrl));
}