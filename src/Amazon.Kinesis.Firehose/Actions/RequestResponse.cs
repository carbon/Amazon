using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class RequestResponse
{
    [JsonPropertyName("ErrorCode")]
    public string? ErrorCode { get; init; }

    [JsonPropertyName("ErrorMessage")]
    public string? ErrorMessage { get; init; }

    [JsonPropertyName("RecordId")]
    public required string RecordId { get; init; }
}