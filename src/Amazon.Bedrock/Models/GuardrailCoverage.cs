namespace Amazon.Bedrock.Models;
using System.Text.Json.Serialization;

public sealed class GuardrailCoverage
{
    [JsonPropertyName("images")]
    public GuardrailCoverageCounts Images { get; set; }

    [JsonPropertyName("textCharacters")]
    public GuardrailCoverageCounts TextCharacters { get; set; }
}