using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class RerankDocument
{
    [JsonPropertyName("jsonDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonElement? JsonDocument { get; init; }

    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RerankTextDocument? TextDocument { get; init; }

    [JsonPropertyName("type")]
    public required RerankDocumentType Type { get; init; }
}