using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class GuardrailCoverageCounts
{
    [JsonPropertyName("guarded")]
    public int Guarded { get; init; }

    [JsonPropertyName("total")]
    public int Total { get; init; }
}