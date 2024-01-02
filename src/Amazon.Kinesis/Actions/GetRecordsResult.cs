using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

public sealed class GetRecordsResult : KinesisResult
{
    [JsonPropertyName("MillisBehindLatest")]
    public int MillisBehindLatest { get; init; }

    [JsonPropertyName("NextShardIterator")]
    public string? NextShardIterator { get; init; }

    [JsonPropertyName("Records")]
    public required List<Record> Records { get; init; }
}
