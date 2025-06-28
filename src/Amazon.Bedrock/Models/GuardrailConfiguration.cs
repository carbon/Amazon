namespace Amazon.Bedrock.Models;

using System.Text.Json.Serialization;

public class GuardrailConfiguration
{
    [JsonPropertyName("guardrailIdentifier")]
    public string? GuardrailIdentifier { get; init; }

    [JsonPropertyName("guardrailVersion")]
    public string? GuardrailVersion { get; init; }

    [JsonPropertyName("trace")]
    public string? Trace { get; init; }
}