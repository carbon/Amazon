using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public sealed class ListTagsForResourceRequest
{
    [JsonPropertyName("resourceArn")]
    public required string ResourceArn { get; init; }
}
