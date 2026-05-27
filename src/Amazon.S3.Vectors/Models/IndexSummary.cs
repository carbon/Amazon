using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors.Models;

public class IndexSummary
{
    [JsonPropertyName("vectorBucketName")]
    public required string VectorBucketName { get; init; }

    [JsonPropertyName("indexName")]
    public required string IndexName { get; init; }

    [JsonPropertyName("indexArn")]
    public required string IndexArn { get; init; }

    [JsonPropertyName("creationTime")]
    public required DateTimeOffset CreationTime { get; init; }
}
