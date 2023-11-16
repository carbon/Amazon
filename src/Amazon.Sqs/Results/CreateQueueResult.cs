using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class CreateQueueResult
{
    [JsonPropertyName("QueueUrl")]
    public required string QueueUrl { get; init; }
}