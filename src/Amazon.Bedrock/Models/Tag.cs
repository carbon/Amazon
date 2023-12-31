using System.Text.Json.Serialization;

namespace Amazon.Bedrock;

public readonly struct Tag
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }
}
