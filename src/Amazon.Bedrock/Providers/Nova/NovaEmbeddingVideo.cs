using System.Text.Json.Serialization;

using Amazon.Bedrock.Models;

namespace Amazon.Bedrock.Nova;

public sealed class NovaEmbeddingVideo
{
    [JsonPropertyName("embeddingMode")]
    public NovaEmbeddingVideoMode EmbeddingMode { get; set; }

    [JsonPropertyName("format")]
    public required string Format { get; set; }

    [JsonPropertyName("source")]
    public required BlobSource SourceObject { get; set; }
}