using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public class ListVectorBucketsResult
{
    [JsonPropertyName("vectorBuckets")]
    public List<VectorBucketSummary> VectorBuckets { get; init; } = new();

    [JsonPropertyName("nextToken")]
    public string? NextToken { get; init; }
}
