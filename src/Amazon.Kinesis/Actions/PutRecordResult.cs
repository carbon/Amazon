namespace Amazon.Kinesis;

public sealed class PutRecordResult : KinesisResult
{
    public string? EncryptionType { get; init; }

    public required string SequenceNumber { get; init; }

    public required string ShardId { get; init; }
}