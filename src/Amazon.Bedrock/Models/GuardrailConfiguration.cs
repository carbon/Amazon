namespace Amazon.Bedrock.Models;
using System.Text.Json.Serialization;

public class GuardrailConfiguration
{
    [JsonPropertyName("guardrailIdentifier")]
    public string? GuardrailIdentifier { get; set; }

    [JsonPropertyName("guardrailVersion")]
    public string? GuardrailVersion { get; set; }

    [JsonPropertyName("trace")]
    public string? Trace { get; set; }
}
