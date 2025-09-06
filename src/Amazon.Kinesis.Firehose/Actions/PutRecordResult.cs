using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class PutRecordResult
{
    [JsonPropertyName("Encrypted")]
    public bool Encrypted { get; init; }

    [JsonPropertyName("RecordId")]
    public required string RecordId { get; init; }
}