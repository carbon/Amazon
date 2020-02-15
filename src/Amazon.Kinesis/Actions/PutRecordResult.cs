#nullable disable

namespace Amazon.Kinesis
{
    public sealed class PutRecordResult : KinesisResponse
    {
        public string SequenceNumber { get; set; }

        public string ShardId { get; set; }
    }
}

/*
{
  "SequenceNumber" : "string",
  "ShardId"        : "string"
}
*/