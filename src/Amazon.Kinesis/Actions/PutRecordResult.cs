#nullable disable

namespace Amazon.Kinesis;

public sealed class PutRecordResult : KinesisResponse
{
    public string SequenceNumber { get; init; }

    public string ShardId { get; init; }
}

/*
{
  "SequenceNumber" : "string",
  "ShardId"        : "string"
}
*/