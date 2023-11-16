using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class BatchResultErrorEntry
{
    [JsonPropertyName("Code")]
    public required string Code { get; init; }

    [JsonPropertyName("Id")]
    public required string Id { get; init; }

    [JsonPropertyName("SenderFault")]
    public required bool SenderFault { get; init; }

    [JsonPropertyName("Message")]
    public string? Message { get; init; }
}