using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailInvocationMetrics
{
    [JsonPropertyName("guardrailCoverage")]
    public GuardrailCoverage? GuardrailCoverage { get; set; }

    [JsonPropertyName("guardrailProcessingLatency")]
    public double GuardrailProcessingLatency { get; set; }

    [JsonPropertyName("usage")]
    public GuardrailUsage? Usage { get; set; }
}