using System.Text.Json.Serialization;

namespace Amazon.Nova;

public sealed class NovaUsage
{
    [JsonPropertyName("inputTokens")]
    public int InputTokens { get; init; }

    [JsonPropertyName("outputTokens")]
    public int OutputTokens { get; init; }

    [JsonPropertyName("totalTokens")]
    public int TotalTokens { get; init; }

    [JsonPropertyName("cacheReadInputTokenCount")]
    public int? CacheReadInputTokenCount { get; init; }

    [JsonPropertyName("cacheWriteInputTokenCount")]
    public int? CacheOutputInputTokenCount { get; init; }
}