using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public readonly struct TokenUsage
{
    [JsonPropertyName("cacheReadInputTokens")]
    public int? CacheReadInputTokens { get; init; }

    [JsonPropertyName("cacheWriteInputTokens")]
    public int? CacheWriteInputTokens { get; init; }

    [JsonPropertyName("inputTokens")]
    public required int InputTokens { get; init; }

    [JsonPropertyName("outputTokens")]
    public required int OutputTokens { get; init; }

    [JsonPropertyName("totalTokens")]
    public int TotalTokens { get; init; }
}