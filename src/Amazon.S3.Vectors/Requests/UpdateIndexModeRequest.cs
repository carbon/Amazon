using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class UpdateIndexModeRequest
{
    [JsonPropertyName("indexArn")]
    public string? IndexArn { get; init; }

    [JsonPropertyName("indexName")]
    public string? IndexName { get; init; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; init; }

    [JsonPropertyName("indexMode")]
    public required IndexMode IndexMode { get; init; }
}
