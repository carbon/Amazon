using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class RerankSource
{
    [JsonPropertyName("inlineDocumentSource")]
    public required RerankDocument InlineDocumentSource { get; init; }

    [JsonPropertyName("type")]
    public required RerankSourceType Type { get; init; }

    public static implicit operator RerankSource(RerankDocument document)
    {
        return new RerankSource { InlineDocumentSource = document, Type = RerankSourceType.Inline };
    }

    public static implicit operator RerankSource(string text)
    {
        return new RerankSource { 
            InlineDocumentSource = new RerankDocument {
                Type = RerankDocumentType.Text,
                TextDocument = text 
            }, 
            Type = RerankSourceType.Inline
        };
    }

    public static implicit operator RerankSource(JsonElement json)
    {
        return new RerankSource {
            InlineDocumentSource = new RerankDocument {
                Type = RerankDocumentType.Json,
                JsonDocument = json
            },
            Type = RerankSourceType.Inline
        };
    }
}
