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

        public string ExclusiveStartShardId { get; set; }

        public int? Limit { get; set; }

        public string StreamName { get; set; }
    }
}

/*
{
    "ExclusiveStartShardId": "string",
    "Limit": "number",
    "StreamName": "string"
}
*/