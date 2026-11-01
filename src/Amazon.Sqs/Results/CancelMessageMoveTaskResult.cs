using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class CancelMessageMoveTaskResult
{
    [JsonPropertyName("ApproximateNumberOfMessagesMoved")]
    public long ApproximateNumberOfMessagesMoved { get; init; }
}
