#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.S3.Events;

public sealed class S3EventDetails
{
    [JsonPropertyName("s3SchemaVersion")]
    public required string S3SchemaVersion { get; init; }

    [JsonPropertyName("configurationId")]
    public string ConfigurationId { get; init; }

    [JsonPropertyName("bucket")]
    public S3EventBucketInfo Bucket { get; init; }

    [JsonPropertyName("object")]
    public S3EventObjectInfo Object { get; init; }
}