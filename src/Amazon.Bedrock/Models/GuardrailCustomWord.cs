using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public readonly struct GuardrailCustomWord
{
    // BLOCKED
    [JsonPropertyName("action")]
    public required string Action { get; init; }

    [JsonPropertyName("match")]
    public required string Match { get; init; }
}