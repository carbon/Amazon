using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class DecryptResult : KmsResult
{
    [JsonPropertyName("KeyId")]
    public required string KeyId { get; init; }

    [JsonPropertyName("EncryptionAlgorithm")]
    public EncryptionAlgorithm EncryptionAlgorithm { get; set; }

    [JsonPropertyName("Plaintext")]
    [MaxLength(4_096)]
    public required byte[] Plaintext { get; init; }
}