using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class StartMessageMoveTaskRequest : SqsRequest
{
    /// <param name="sourceArn">The ARN of the dead-letter queue to move messages from</param>
    /// <param name="destinationArn">The ARN of the receiving queue. Defaults to the original source queues</param>
    /// <param name="maxNumberOfMessagesPerSecond">The movement rate, up to 500. Defaults to a rate optimized by SQS</param>
    public StartMessageMoveTaskRequest(string sourceArn, string? destinationArn = null, int? maxNumberOfMessagesPerSecond = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(sourceArn);

        if (maxNumberOfMessagesPerSecond is <= 0 or > 500)
        {
            throw new ArgumentOutOfRangeException(nameof(maxNumberOfMessagesPerSecond), maxNumberOfMessagesPerSecond, "Must be between 1 and 500");
        }

        SourceArn = sourceArn;
        DestinationArn = destinationArn;
        MaxNumberOfMessagesPerSecond = maxNumberOfMessagesPerSecond;
    }

    [JsonPropertyName("DestinationArn")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DestinationArn { get; }

    [JsonPropertyName("MaxNumberOfMessagesPerSecond")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxNumberOfMessagesPerSecond { get; }

    [JsonPropertyName("SourceArn")]
    public string SourceArn { get; }
}
