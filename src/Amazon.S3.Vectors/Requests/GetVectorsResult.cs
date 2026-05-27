using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class GetVectorsResult
{
    [JsonPropertyName("vectors")]
    public List<GetOutputVector> Vectors { get; init; } = new();
}
