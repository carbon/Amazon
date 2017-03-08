using System;
using Carbon.Data.Streams;

namespace Amazon.Kinesis
{
    public class Shard : IShard
    {
        public Shard() { }

        public Shard(string id)
        {
            ShardId = id ?? throw new ArgumentNullException(nameof(id));
        }

        public string AdjacentParentShardId { get; set; }

        public string ParentShardId { get; set; }

        public HashKeyRange HashKeyRange { get; set; }

        public SequenceNumberRange SequenceNumberRange { get; set; }

        public string ShardId { get; set; }

        #region IShard

        string IShard.Id => ShardId;

        #endregion
    }

    public class HashKeyRange
    {
        public string StartingHashKey { get; set; }

        public string EndingHashKey { get; set; }
    }

    public class SequenceNumberRange
    {
        public string StartingSequenceNumber { get; set; }

        public string EndingSequenceNumber { get; set; }
    }
}