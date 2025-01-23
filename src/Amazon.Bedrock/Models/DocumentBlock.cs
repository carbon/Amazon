using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class DocumentBlock
{
    [JsonPropertyName("format")]
    public required string Format { get; init; }

    // docs only
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("source")]
    public required BlobSource Source { get; init; }
}