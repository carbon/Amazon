using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class PutVectorsRequest
{
    [JsonPropertyName("indexArn")]
    public string? IndexArn { get; init; }

    [JsonPropertyName("indexName")]
    public string? IndexName { get; init; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; init; }

    [JsonPropertyName("vectors")]
    public required IReadOnlyList<PutInputVector> Vectors { get; init; }
}