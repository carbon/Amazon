using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class TagResourceRequest
{
    [JsonPropertyName("resourceArn")]
    public required string ResourceArn { get; set; }

    [JsonPropertyName("tags")]
    public required Dictionary<string, string> Tags { get; set; }
}
