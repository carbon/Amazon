using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class BufferingHints
{
    [JsonPropertyName("IntervalInSeconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Range(0, 900)]
    public int? IntervalInSeconds { get; init; }

    [JsonPropertyName("SizeInMBs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Range(1, 128)]
    public int? SizeInMBs { get; init; }
}