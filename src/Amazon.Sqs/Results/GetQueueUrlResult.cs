using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class GetQueueUrlResult
{
    [JsonPropertyName("QueueUrl")]
    public required string QueueUrl { get; init; }
}
