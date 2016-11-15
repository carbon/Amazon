namespace Amazon.Kinesis
{
    public class DescribeStreamRequest : KinesisRequest
    {
        public DescribeStreamRequest() { }

        public DescribeStreamRequest(string streamName)
        {
            StreamName = streamName;
        }

        public string ExclusiveStartShardId { get; set; }

        public int? Limit { get; set; }

        public string StreamName { get; set; }
    }

    public class DescribeStreamResult
    {
        public StreamDescription StreamDescription { get; set; }
    }
}
/*
REQUEST
{
    "ExclusiveStartShardId": "string",
    "Limit": "number",
    "StreamName": "string"
}

RESPONSE
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
