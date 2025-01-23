using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailManagedWord
{
    [JsonPropertyName("action")]
    public required string Action { get; init; }

    [JsonPropertyName("match")]
    public required string Match { get; init; }

    // PROFANITY
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}