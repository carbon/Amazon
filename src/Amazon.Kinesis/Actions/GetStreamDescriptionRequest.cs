#nullable disable

using System;

namespace Amazon.Kinesis
{
    public sealed class DescribeStreamRequest : KinesisRequest
    {
        public DescribeStreamRequest() { }

        public DescribeStreamRequest(string streamName)
        {
            StreamName = streamName ?? throw new ArgumentNullException(nameof(streamName));
        }

        public string ExclusiveStartShardId { get; init; }

        public int? Limit { get; init; }

        public string StreamName { get; init; }
    }
}

/*
{
    "ExclusiveStartShardId": "string",
    "Limit": "number",
    "StreamName": "string"
}
*/