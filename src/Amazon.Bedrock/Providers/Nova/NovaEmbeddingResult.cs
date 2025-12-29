using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

public sealed class NovaEmbeddingResult
{
    [JsonPropertyName("embeddings")]
    public required NovaEmbedding[] Embeddings { get; init; }
}