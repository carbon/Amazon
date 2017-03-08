namespace Amazon.Kinesis
{
    using System;

    public class GetShardIteratorRequest : KinesisRequest
    {
        public GetShardIteratorRequest(string streamName, string shardId,
            ShardIteratorType type, string startingSequenceNumber = null)
        {
            StreamName = streamName ?? throw new ArgumentNullException(streamName);
            ShardId = shardId ?? throw new ArgumentNullException(shardId);
            ShardIteratorType = type;
            StartingSequenceNumber = startingSequenceNumber;
        }

        public string ShardId { get; }

        public ShardIteratorType ShardIteratorType { get; }

        public string StartingSequenceNumber { get; }

        public string StreamName { get; }
    }


    public enum ShardIteratorType
    {
        /// <summary>
        /// Start reading exactly from the position denoted by a specific sequence number.
        /// </summary>
        AT_SEQUENCE_NUMBER,

        /// <summary>
        /// Start reading right after the position denoted by a specific sequence number.
        /// </summary>
        AFTER_SEQUENCE_NUMBER,

        /// <summary>
        /// Start reading at the last untrimmed record in the shard in the system, which is the oldest data record in the shard.
        /// </summary>
        TRIM_HORIZON,

        /// <summary>
        /// Start reading just after the most recent record in the shard, so that you always read the most recent data in the shard.
        /// </summary>
        LATEST
    }
}