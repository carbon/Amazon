using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailInvocationMetrics
{
    [JsonPropertyName("guardrailCoverage")]
    public GuardrailCoverage? GuardrailCoverage { get; init; }

    [JsonPropertyName("guardrailProcessingLatency")]
    public double GuardrailProcessingLatency { get; init; }

    [JsonPropertyName("usage")]
    public GuardrailUsage? Usage { get; init; }
}