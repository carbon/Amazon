#nullable disable

using System;

using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class Shard : IShard
{
    public Shard() { }

    public Shard(string id)
    {
        ShardId = id ?? throw new ArgumentNullException(nameof(id));
    }

    public string AdjacentParentShardId { get; init; }

    public string ParentShardId { get; init; }

    public HashKeyRange HashKeyRange { get; init; }

    public SequenceNumberRange SequenceNumberRange { get; init; }

    public string ShardId { get; init; }

    // IShard

    string IShard.Id => ShardId;
}
