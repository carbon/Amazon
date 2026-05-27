using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors.Models;

public sealed class EncryptionConfiguration
{
    [JsonPropertyName("kmsKeyArn")]
    public string? KmsKeyArn { get; init; }

    [JsonPropertyName("sseType")]
    public string SseType { get; init; } = default!; // AES256 | aws:kms
}
