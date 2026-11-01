using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ListMessageMoveTasksResultEntry
{
    [JsonPropertyName("ApproximateNumberOfMessagesMoved")]
    public long ApproximateNumberOfMessagesMoved { get; init; }

    [JsonPropertyName("ApproximateNumberOfMessagesToMove")]
    public long? ApproximateNumberOfMessagesToMove { get; init; }

    [JsonPropertyName("DestinationArn")]
    public string? DestinationArn { get; init; }

    [JsonPropertyName("FailureReason")]
    public string? FailureReason { get; init; }

    [JsonPropertyName("MaxNumberOfMessagesPerSecond")]
    public int? MaxNumberOfMessagesPerSecond { get; init; }

    [JsonPropertyName("SourceArn")]
    public string? SourceArn { get; init; }

    /// <summary>
    /// The start time, in milliseconds since the Unix epoch
    /// </summary>
    [JsonPropertyName("StartedTimestamp")]
    public long StartedTimestamp { get; init; }

    [JsonPropertyName("Status")]
    public string? Status { get; init; }

    /// <summary>
    /// Only present while the task is running
    /// </summary>
    [JsonPropertyName("TaskHandle")]
    public string? TaskHandle { get; init; }
}
