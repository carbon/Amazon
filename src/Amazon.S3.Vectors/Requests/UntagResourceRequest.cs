using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class UntagResourceRequest
{
    [JsonPropertyName("resourceArn")]
    public string ResourceArn { get; init; } = default!;

    [JsonPropertyName("tagKeys")]
    public List<string> TagKeys { get; init; } = new();
}
