using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class CachePointBlock
{
    public static readonly CachePointBlock Default = new() { Type = "default"};
    
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}