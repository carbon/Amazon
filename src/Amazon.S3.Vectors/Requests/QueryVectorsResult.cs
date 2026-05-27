using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class QueryVectorsResult
{
    [JsonPropertyName("distanceMetric")]
    public DistanceMetric DistanceMetric { get; init; }

    [JsonPropertyName("vectors")]
    public List<QueryOutputVector> Vectors { get; init; } = null!;
}
