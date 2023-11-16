﻿#nullable disable

namespace Amazon.Kinesis;

public sealed class DescribeStreamResult : KinesisResult
{
    public StreamDescription StreamDescription { get; init; }
}

/*
{
    "StreamDescription": {
        "HasMoreShards": boolean,
        "Shards": [
            {
                "AdjacentParentShardId": "string",
                "HashKeyRange": {
                    "EndingHashKey": "string",
                    "StartingHashKey": "string"
                },
                "ParentShardId": "string",
                "SequenceNumberRange": {
                    "EndingSequenceNumber": "string",
                    "StartingSequenceNumber": "string"
                },
                "ShardId": "string"
            }
        ],
        "StreamARN": "string",
        "StreamName": "string",
        "StreamStatus": "string"
    }
}
*/
