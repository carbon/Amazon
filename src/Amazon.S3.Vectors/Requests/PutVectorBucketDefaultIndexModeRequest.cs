using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class PutVectorBucketDefaultIndexModeRequest
{
    [JsonPropertyName("vectorBucketArn")]
    public string? VectorBucketArn { get; init; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; init; }

    [JsonPropertyName("defaultIndexMode")]
    public required IndexMode DefaultIndexMode { get; init; }
}
