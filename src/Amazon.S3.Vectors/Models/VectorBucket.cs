using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors.Models;

public class Index
{
    [JsonPropertyName("vectorBucketName")]
    public required string VectorBucketName { get; init; }

    [JsonPropertyName("indexName")]
    public required string IndexName { get; init; }

    [JsonPropertyName("indexArn")]
    public required string IndexArn { get; init ; }

    [JsonPropertyName("creationTime")]
    public DateTimeOffset CreationTime { get; init; }

    [JsonPropertyName("dataType")]
    public string DataType { get; init; } = default!;

    [JsonPropertyName("dimension")]
    public int Dimension { get; init; }

    [JsonPropertyName("distanceMetric")]
    public DistanceMetric DistanceMetric { get; init; }

    [JsonPropertyName("metadataConfiguration")]
    public MetadataConfiguration? MetadataConfiguration { get; init; }

    [JsonPropertyName("encryptionConfiguration")]
    public EncryptionConfiguration? EncryptionConfiguration { get; init; }
}

public class VectorBucketSummary
{
    [JsonPropertyName("vectorBucketName")]
    public string VectorBucketName { get; init; } = default!;

    [JsonPropertyName("vectorBucketArn")]
    public string VectorBucketArn { get; init; } = default!;

    [JsonPropertyName("creationTime")]
    public DateTimeOffset CreationTime { get; init; }
}

public sealed class VectorBucket
{
    [JsonPropertyName("vectorBucketName")]
    public string VectorBucketName { get; init; } = default!;

    [JsonPropertyName("vectorBucketArn")]
    public string VectorBucketArn { get; init; } = default!;

    [JsonPropertyName("creationTime")]
    public required DateTime CreationTime { get; init; }

    [JsonPropertyName("encryptionConfiguration")]
    public EncryptionConfiguration? EncryptionConfiguration { get; init; }
}

public class GetOutputVector
{
    [JsonPropertyName("key")]
    public string Key { get; init; } = default!;

    [JsonPropertyName("data")]
    public VectorData? Data { get; init; }

    [JsonPropertyName("metadata")]
    public JsonElement? Metadata { get; init; }
}


public class ListOutputVector
{
    [JsonPropertyName("key")]
    public string Key { get; init; } = default!;

    [JsonPropertyName("data")]
    public VectorData? Data { get; init; }

    [JsonPropertyName("metadata")]
    public JsonElement? Metadata { get; init; }
}

public class QueryOutputVector
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("distance")]
    public float? Distance { get; init; }

    [JsonPropertyName("metadata")]
    public JsonElement? Metadata { get; init; }
}

public class ValidationExceptionField
{
    [JsonPropertyName("message")]
    public string Message { get; init; } = default!;

    [JsonPropertyName("path")]
    public string Path { get; init; } = default!;
}
