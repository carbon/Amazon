using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class GuardrailCoverageCounts
{
    [JsonPropertyName("guarded")]
    public int Guarded { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}