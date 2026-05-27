using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public sealed class ListVectorBucketsRequest
{
    [JsonPropertyName("prefix")]
    public string? Prefix { get; init; }

    [JsonPropertyName("maxResults")]
    public int? MaxResults { get; init; }

    [JsonPropertyName("nextToken")]
    public string? NextToken { get; init; }
}
