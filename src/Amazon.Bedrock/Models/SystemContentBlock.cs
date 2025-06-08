namespace Amazon.Bedrock.Models;

using System.Text.Json.Serialization;

public sealed class SystemContentBlock
{
    [JsonPropertyName("cachePoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CachePointBlock? CachePoint { get; init; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }

    public static implicit operator SystemContentBlock(string text)
    {
        return new SystemContentBlock { Text = text };
    }

    public static implicit operator SystemContentBlock(CachePointBlock cachePoint)
    {
        return new SystemContentBlock { CachePoint = cachePoint };
    }
}