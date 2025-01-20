using System.Text.Json.Serialization;

namespace Amazon.Nova;

public sealed class NovaUsage
{
    [JsonPropertyName("inputTokens")]
    public int InputTokens { get; set; }

    [JsonPropertyName("outputTokens")]
    public int OutputTokens { get; set; }

    [JsonPropertyName("totalTokens")]
    public int TotalTokens { get; set; }

    [JsonPropertyName("cacheReadInputTokenCount")]
    public int? CacheReadInputTokenCount { get; set; }

    [JsonPropertyName("cacheWriteInputTokenCount")]
    public int? CacheOutputInputTokenCount { get; set; }
}