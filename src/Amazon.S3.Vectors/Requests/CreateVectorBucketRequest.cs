using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class CreateVectorBucketRequest
{
    [JsonPropertyName("vectorBucketName")]
    public required string VectorBucketName { get; set; }

    [JsonPropertyName("encryptionConfiguration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EncryptionConfiguration? EncryptionConfiguration { get; set; }

    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Tags { get; set; }
}
