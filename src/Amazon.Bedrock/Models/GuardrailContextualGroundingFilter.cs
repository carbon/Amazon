using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailContextualGroundingFilter
{
    // BLOCKED | NONE
    [JsonPropertyName("action")]
    public required string Action { get; set; }

    [JsonPropertyName("score")]
    public required double Score { get; set; }

    [JsonPropertyName("threshold")]
    public required double Threshold { get; set; }

    // GROUNDING | RELEVANCE
    [JsonPropertyName("type")]
    public required string Type { get; set; }
}