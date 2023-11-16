using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

public sealed class MergeShardsRequest : KinesisRequest
{
    [JsonPropertyName("AdjacentShardToMerge")]
    public required string AdjacentShardToMerge { get; init; }

    [JsonPropertyName("ShardToMerge")]
    public required string ShardToMerge { get; init; }

    [JsonPropertyName("StreamARN")]
    public string? StreamArn { get; init; }

    [JsonPropertyName("StreamName")]
    public string? StreamName { get; init; }
}