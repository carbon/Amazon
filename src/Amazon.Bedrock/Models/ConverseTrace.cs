using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class ConverseTrace
{
    [JsonPropertyName("guardrail")]
    public GuardrailTraceAssessment? Guardrail { get; init; }

    [JsonPropertyName("promptRouter")]
    public PromptRouterTrace? PromptRouter { get; init; }
}