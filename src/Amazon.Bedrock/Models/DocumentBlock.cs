using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class DocumentBlock
{
    [JsonPropertyName("format")]
    public required string Format { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("source")]
    public required BlobSource Source { get; init; }
}