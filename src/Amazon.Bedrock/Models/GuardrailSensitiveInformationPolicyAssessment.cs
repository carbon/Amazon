using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class GuardrailSensitiveInformationPolicyAssessment
{
    [JsonPropertyName("piiEntities")]
    public required List<GuardrailPiiEntityFilter> PiiEntities { get; set; }

    [JsonPropertyName("regexes")]
    public required List<RegexMatch> Regexes { get; set; }
}