namespace Amazon.Bedrock.Models;
using System.Text.Json.Serialization;

public class VideoBlock
{
    // "mkv" | "mov" | "mp4" | "webm" | "three_gp" | "flv" | "mpeg" | "mpg" | "wmv"
    [JsonPropertyName("format")]
    public required string Format { get; init; }

    [JsonPropertyName("source")]
    public required BlobSource Source { get; init; }
}
