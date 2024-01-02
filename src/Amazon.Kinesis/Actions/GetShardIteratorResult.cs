using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class GetShardIteratorResult : KinesisResult, IIterator
{
    public required string ShardIterator { get; init; }

    // IIterator
    string IIterator.Value => ShardIterator;
}