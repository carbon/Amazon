using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public readonly struct DeleteMessageBatchResultEntry
{
    [JsonPropertyName("Id")]
    public required string Id { get; init; } 
}