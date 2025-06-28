using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailContextualGroundingPolicyAssessment
{
    [JsonPropertyName("filters")]
    public List<GuardrailContextualGroundingFilter>? Filters { get; init; }
}