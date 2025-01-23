using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailContentPolicyAssessment
{
    [JsonPropertyName("filters")]
    public required List<GuardrailContentFilter> Filters { get; set; }
}