using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public class ListVectorsResult
{
    [JsonPropertyName("vectors")]
    public List<ListOutputVector> Vectors { get; init; } = new();

    [JsonPropertyName("nextToken")]
    public string? NextToken { get; init; }
}
