using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class StartMessageMoveTaskResult
{
    [JsonPropertyName("TaskHandle")]
    public required string TaskHandle { get; init; }
}
