using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class EncryptResult : KmsResult
{
    [JsonPropertyName("CiphertextBlob")]
    public required byte[] CiphertextBlob { get; init; }

    [JsonPropertyName("EncryptionAlgorithm")]
    public EncryptionAlgorithm EncryptionAlgorithm { get; set; }

    [JsonPropertyName("KeyId")]
    public required string KeyId { get; init; }
}