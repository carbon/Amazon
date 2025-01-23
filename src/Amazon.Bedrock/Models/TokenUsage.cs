using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public readonly struct TokenUsage
{
    [JsonPropertyName("inputTokens")]
    public required int InputTokens { get; init; }

    [JsonPropertyName("outputTokens")]
    public required int OutputTokens { get; init; }

    [JsonPropertyName("totalTokens")]
    public int TotalTokens { get; init; }
}