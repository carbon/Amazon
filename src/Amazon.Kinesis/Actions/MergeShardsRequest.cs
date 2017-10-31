namespace Amazon.Kinesis
{
    public class MergeShardsRequest : KinesisRequest
    {
        public string AdjacentShardToMerge { get; set; }

        public string ShardToMerge { get; set; }

        public string StreamName { get; set; }
    }

    public class MergeShardResult
    {
    }
}

/*
{
    "AdjacentShardToMerge": "string",
    "ShardToMerge": "string",
    "StreamName": "string"
}
*/

/*
In order to merge two shards, the shards must be adjacent. Two shards are considered adjacent if the union of the hash key ranges for the two shards form a contiguous set with no gaps. For example, if you have two shards, one with a hash key range of 276...381 and the other with a hash key range of 382...454, then you could merge these two shards into a single shard that would have a hash key range of 276...454.

To take another example, if you have two shards, one with a hash key range of 276..381 and the other with a hash key range of 455...560, then you could not merge these two shards because there would be one or more shards between these two that cover the range 382..454.

The set of all OPEN shards in a stream—as a group—always spans the entire range of MD5 hash key values. For more information about shard states—such as CLOSED—see Data Routing, Data Persistence, and Shard State after a Reshard.

To identify shards that are candidates for merging, you should filter out all shards that are in a CLOSED state. Shards that are OPEN—that is, not CLOSED—have an ending sequence number of null. You can test the ending sequence number for a shard using:

if( null == shard.getSequenceNumberRange().getEndingSequenceNumber() ) {
  // Shard is OPEN, so it is a possible candidate to be merged.
}
After filtering out the closed shards, sort the remaining shards by the highest hash key value supported by each shard. You can retrieve this value using:

shard.getHashKeyRange().getEndingHashKey();
*/
