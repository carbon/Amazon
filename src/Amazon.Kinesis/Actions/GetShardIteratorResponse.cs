#nullable disable

using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class GetShardIteratorResponse : KinesisResponse, IIterator
{
    public string ShardIterator { get; init; }

    // IIterator
    string IIterator.Value => ShardIterator;
}
