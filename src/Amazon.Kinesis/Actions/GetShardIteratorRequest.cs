#nullable enable

using System;

namespace Amazon.Kinesis
{
    public class GetShardIteratorRequest : KinesisRequest
    {
        public GetShardIteratorRequest(
            string streamName, 
            string shardId,
            ShardIteratorType type,
            string? startingSequenceNumber = null)
        {
            StreamName             = streamName ?? throw new ArgumentNullException(streamName);
            ShardId                = shardId ?? throw new ArgumentNullException(shardId);
            ShardIteratorType      = type;
            StartingSequenceNumber = startingSequenceNumber;
        }

        public string ShardId { get; }

        public ShardIteratorType ShardIteratorType { get; }

        public string? StartingSequenceNumber { get; }

        public string StreamName { get; }
    }
}