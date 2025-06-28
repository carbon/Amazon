using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailContextualGroundingFilter
{
    // BLOCKED | NONE
    [JsonPropertyName("action")]
    public required string Action { get; init; }

    [JsonPropertyName("score")]
    public required double Score { get; init; }

    [JsonPropertyName("threshold")]
    public required double Threshold { get; init; }

    // GROUNDING | RELEVANCE
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}