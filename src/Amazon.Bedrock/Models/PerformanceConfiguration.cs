namespace Amazon.Bedrock.Models;
using System.Text.Json.Serialization;

public class PerformanceConfiguration
{
    public static readonly PerformanceConfiguration Optimized = new() { Latency = "optimized" };
    public static readonly PerformanceConfiguration Standard = new() { Latency = "standard" };

    // standard | optimized
    [JsonPropertyName("latency")]
    public required string Latency { get; init; }
}