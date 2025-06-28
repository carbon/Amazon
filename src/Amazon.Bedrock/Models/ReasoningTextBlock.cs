using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class ReasoningTextBlock
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    [JsonPropertyName("signature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Signature { get; init; }
}