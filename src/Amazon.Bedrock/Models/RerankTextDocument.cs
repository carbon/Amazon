using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class RerankTextDocument
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    public static implicit operator RerankTextDocument(string text)
    {
        return new RerankTextDocument { Text = text };
    }
}
