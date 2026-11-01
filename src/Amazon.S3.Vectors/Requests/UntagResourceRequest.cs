using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class UntagResourceRequest
{
    [JsonPropertyName("resourceArn")]
    public required string ResourceArn { get; init; }

    [JsonPropertyName("tagKeys")]
    public required List<string> TagKeys { get; init; }
}
