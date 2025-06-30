using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class ImageBlock
{
    // "jpeg" | "png" | "gif" | "webp"
    [JsonPropertyName("format")]
    public required string Format { get; init; }

    [JsonPropertyName("source")]
    public required BlobSource Source { get; init; }
}