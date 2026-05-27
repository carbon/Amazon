using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class TagResourceRequest
{
    [JsonPropertyName("resourceArn")]
    public string ResourceArn { get; set; } = default!;

    [JsonPropertyName("tags")]
    public Dictionary<string, string> Tags { get; set; } = new();
}
