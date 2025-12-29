using System.Text.Json.Serialization;

using Amazon.Bedrock.Models;

namespace Amazon.Bedrock.Nova;

public sealed class NovaEmbeddingAudio
{
    [JsonPropertyName("format")]
    public required string Format { get; set; }

    [JsonPropertyName("source")]
    public required BlobSource SourceObject { get; set; }
}