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
    [JsonConverter(typeof(UnixTimestampConverter))]
    public required DateTimeOffset CreationTime { get; init; }

    [JsonPropertyName("dataType")]
    public required string DataType { get; init; }

    [JsonPropertyName("dimension")]
    public required int Dimension { get; init; }

    [JsonPropertyName("distanceMetric")]
    public required DistanceMetric DistanceMetric { get; init; }

    [JsonPropertyName("metadataConfiguration")]
    public MetadataConfiguration? MetadataConfiguration { get; init; }

    [JsonPropertyName("encryptionConfiguration")]
    public EncryptionConfiguration? EncryptionConfiguration { get; init; }

    [JsonPropertyName("indexMode")]
    public IndexMode? IndexMode { get; init; }
}

public class VectorBucketSummary
{
    [JsonPropertyName("vectorBucketName")]
    public required string VectorBucketName { get; init; }

    [JsonPropertyName("vectorBucketArn")]
    public required string VectorBucketArn { get; init; }

    [JsonPropertyName("creationTime")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public required DateTimeOffset CreationTime { get; init; }
}

public sealed class VectorBucket
{
    [JsonPropertyName("vectorBucketName")]
    public required string VectorBucketName { get; init; }

    [JsonPropertyName("vectorBucketArn")]
    public required string VectorBucketArn { get; init; }

    [JsonPropertyName("creationTime")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public required DateTimeOffset CreationTime { get; init; }

    [JsonPropertyName("encryptionConfiguration")]
    public EncryptionConfiguration? EncryptionConfiguration { get; init; }

    [JsonPropertyName("defaultIndexMode")]
    public IndexMode? DefaultIndexMode { get; init; }
}

public class GetOutputVector
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("data")]
    public VectorData? Data { get; init; }

    [JsonPropertyName("metadata")]
    public JsonElement? Metadata { get; init; }
}


public class ListOutputVector
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

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
    public required string Message { get; init; }

    [JsonPropertyName("path")]
    public required string Path { get; init; }
}
