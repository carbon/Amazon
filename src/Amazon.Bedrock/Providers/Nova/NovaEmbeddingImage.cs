using System.Text.Json.Serialization;

using Amazon.Bedrock.Models;

namespace Amazon.Bedrock.Nova;

public sealed class NovaEmbeddingImage
{
    [JsonPropertyName("detailLevel")]
    public required NovaEmbeddingImageDetail DetailLevel { get; init; }

    [JsonPropertyName("format")]
    public required string Format { get; init; }

    [JsonPropertyName("source")]
    public required BlobSource SourceObject { get; set; }
}