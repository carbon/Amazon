#nullable disable

using System.Collections.Generic;

namespace Amazon.Kinesis;

public sealed class StreamDescription
{
    public bool HasMoreShards { get; init; }

    public List<Shard> Shards { get; init; }

    public string StreamARN { get; init; }

    public string StreamName { get; init; }

    public string StreamStatus { get; init; }
}

/*
{
    "StreamDescription": {
        "HasMoreShards": "boolean",
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
