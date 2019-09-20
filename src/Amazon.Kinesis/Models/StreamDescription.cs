#nullable disable

using System.Collections.Generic;

namespace Amazon.Kinesis
{
    public class StreamDescription
    {
        public bool HasMoreShards { get; set; }

        public List<Shard> Shards { get; set; }

        public string StreamARN { get; set; }

        public string StreamName { get; set; }

        public string StreamStatus { get; set; }
    }
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
