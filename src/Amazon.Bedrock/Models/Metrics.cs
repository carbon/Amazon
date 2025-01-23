using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public readonly struct Metrics
{
    [JsonPropertyName("latencyMs")]
    public int LatencyMs { get; init; }
}
