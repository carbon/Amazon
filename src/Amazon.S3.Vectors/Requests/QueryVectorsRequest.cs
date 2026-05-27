using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class QueryVectorsRequest
{
    [JsonPropertyName("indexArn")]
    public string? IndexArn { get; init; }

    [JsonPropertyName("indexName")]
    public string? IndexName { get; init; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; init; }

    [JsonPropertyName("queryVector")]
    public required VectorData QueryVector { get; init; }

    [JsonPropertyName("topK")]
    public required int TopK { get; init; }

    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement? Filter { get; init; }

    [JsonPropertyName("returnDistance")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ReturnDistance { get; init; }

    [JsonPropertyName("returnMetadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ReturnMetadata { get; init; }
}
