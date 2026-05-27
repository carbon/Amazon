using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public sealed class ListTagsForResourceResult
{
    [JsonPropertyName("tags")]
    public Dictionary<string, string> Tags { get; init; } = new();
}
