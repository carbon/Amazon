using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class S3Location
{
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    [JsonPropertyName("bucketOwner")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BucketOwner { get; init; }
}