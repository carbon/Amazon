using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class GuardrailSensitiveInformationPolicyAssessment
{
    [JsonPropertyName("piiEntities")]
    public required List<GuardrailPiiEntityFilter> PiiEntities { get; init; }

    [JsonPropertyName("regexes")]
    public required List<GuardrailRegexFilter> Regexes { get; init; }
}