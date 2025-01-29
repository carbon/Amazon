using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class RerankDocument
{
    [JsonPropertyName("jsonDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonElement? JsonDocument { get; set; }

    [JsonPropertyName("textDocument")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RerankTextDocument? TextDocument { get; set; }

    [JsonPropertyName("type")]
    public required RerankDocumentType Type { get; init; }
}