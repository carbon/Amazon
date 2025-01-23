using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailTopic
{
    // BLOCKED
    [JsonPropertyName("action")]
    public required string Action { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    // DENY
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}