using System.Text.Json.Serialization;

using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class GetShardIteratorResult : KinesisResult, IIterator
{
    [JsonPropertyName("ShardIterator")]
    public required string ShardIterator { get; init; }

    // IIterator
    string IIterator.Value => ShardIterator;
}