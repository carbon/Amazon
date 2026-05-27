using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class CreateIndexRequest
{
    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; init; }

    [JsonPropertyName("vectorBucketArn")]
    public string? VectorBucketArn { get; init; }

    [JsonPropertyName("indexName")]
    [MaxLength(63)]
    public required string IndexName { get; init; }

    // must be float32
    [JsonPropertyName("dataType")]
    public required VectorDataType DataType { get; init; }

    // 1-4096
    [JsonPropertyName("dimension")]
    public required int Dimension { get; init; }

    [JsonPropertyName("distanceMetric")]
    public required DistanceMetric DistanceMetric { get; init; }

    [JsonPropertyName("metadataConfiguration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MetadataConfiguration? MetadataConfiguration { get; init; }

    [JsonPropertyName("encryptionConfiguration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EncryptionConfiguration? EncryptionConfiguration { get; init; }

    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Tags { get; init; }
}

