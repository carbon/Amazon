using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class GetVectorBucketResult
{
    [JsonPropertyName("vectorBucket")]
    public VectorBucket VectorBucket { get; init; } = null!;
}