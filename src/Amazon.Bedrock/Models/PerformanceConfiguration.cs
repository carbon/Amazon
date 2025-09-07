using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class PerformanceConfiguration
{
    public static readonly PerformanceConfiguration Optimized = new() { Latency = "optimized" };
    public static readonly PerformanceConfiguration Standard = new() { Latency = "standard" };

    // standard | optimized
    [JsonPropertyName("latency")]
    public required string Latency { get; init; }
}