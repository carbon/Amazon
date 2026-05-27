using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class DeleteVectorsRequest
{
    [JsonPropertyName("indexArn")]
    public string? IndexArn { get; init; }

    [JsonPropertyName("indexName")]
    public string? IndexName { get; init; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; init; }

    [JsonPropertyName("keys")]
    public required IReadOnlyList<string> Keys { get; init; }
}