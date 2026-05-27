using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class ListIndexesResult
{
    [JsonPropertyName("indexes")]
    public List<IndexSummary> Indexes { get; init; } = new();

    [JsonPropertyName("nextToken")]
    public string? NextToken { get; init; }
}
