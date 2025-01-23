using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailContentFilter
{
    [JsonPropertyName("action")]
    public required string Action { get; init; }

    // NONE | LOW | MEDIUM | HIGH
    [JsonPropertyName("confidence")]
    public required string Confidence { get; init; }

    // NONE | LOW | MEDIUM | HIGH
    [JsonPropertyName("filterStrength")]
    public required string FilterStrength { get; init; }

    // INSULTS | HATE | SEXUAL | VIOLENCE | MISCONDUCT | PROMPT_ATTACK
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}