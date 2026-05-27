using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public sealed class GetVectorsRequest
{
    [JsonPropertyName("indexArn")]
    public string? IndexArn { get; init; }

    [JsonPropertyName("indexName")]
    public string? IndexName { get; init; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; init; }

    [JsonPropertyName("keys")]
    public required IReadOnlyList<string> Keys { get; init; }

    [JsonPropertyName("returnData")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ReturnData { get; init; }

    [JsonPropertyName("returnMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ReturnMetadata { get; init; }
}
