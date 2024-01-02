using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

public sealed class MergeShardsRequest : KinesisRequest
{
    [JsonPropertyName("AdjacentShardToMerge")]
    public required string AdjacentShardToMerge { get; init; }

    [JsonPropertyName("ShardToMerge")]
    public required string ShardToMerge { get; init; }

    [JsonPropertyName("StreamARN")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StreamArn { get; init; }

    [JsonPropertyName("StreamName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StreamName { get; init; }
}