using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

public sealed class PutRecordsResult : KinesisResult
{
    public required string EncryptionType { get; init; }

    public int FailedRecordCount { get; init; }

    [JsonPropertyName("Records")]
    public required List<PutRecordsResultEntry> Records { get; init; }
}

public sealed class PutRecordsResultEntry
{
    public string? SequenceNumber { get; init; }

    public string? ShardId { get; init; }

    public string? ErrorCode { get; init; }

    public string? ErrorMessage { get; init; }
}