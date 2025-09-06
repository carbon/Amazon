using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class CloudWatchLoggingOptions
{
    [JsonPropertyName("Enabled")]
    public bool Enabled { get; init; }

    [JsonPropertyName("LogGroupName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LogGroupName { get; init; }

    [JsonPropertyName("LogStreamName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LogStreamName { get; init; }
}