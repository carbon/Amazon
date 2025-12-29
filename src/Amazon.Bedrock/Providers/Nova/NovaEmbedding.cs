using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

public sealed class NovaEmbedding
{
    [JsonPropertyName("embeddingType")]
    public required NovaEmbeddingType EmbeddingType { get; set; }

    [JsonPropertyName("embedding")]
    public required float[] Embedding { get; set; }

    [JsonPropertyName("truncatedCharLength")]
    public int? TruncatedCharLength { get; set; }
}