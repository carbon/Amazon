using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class Shard : IShard
{
    public Shard() { }

    public Shard(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        ShardId = id;
    }

    public string? AdjacentParentShardId { get; init; }

    public string? ParentShardId { get; init; }

    public required HashKeyRange HashKeyRange { get; init; }

    public required SequenceNumberRange SequenceNumberRange { get; init; }

    public required string ShardId { get; init; }

    // IShard

    string IShard.Id => ShardId;
}