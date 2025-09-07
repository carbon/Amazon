using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.Bedrock.Models;

namespace Amazon.Bedrock.Actions;

public class ConverseResult
{
    [JsonPropertyName("additionalModelResponseFields")]
    public JsonElement AdditionalModelResponseFields { get; init; }

    [JsonPropertyName("metrics")]
    public Metrics Metrics { get; init; }

    [JsonPropertyName("output")]
    public ConverseOutput Output { get; init; }

    [JsonPropertyName("performanceConfig")]
    public PerformanceConfiguration? PerformanceConfig { get; init; }

    [JsonPropertyName("stopReason")]
    public StopReason StopReason { get; init; }

    [JsonPropertyName("trace")]
    public ConverseTrace? Trace { get; init; }

    [JsonPropertyName("usage")]
    public TokenUsage Usage { get; init; }
}
