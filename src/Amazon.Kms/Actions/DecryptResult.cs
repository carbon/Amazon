#nullable disable

namespace Amazon.Kms;

public sealed class DecryptResult : KmsResult
{
    public required string KeyId { get; init; }

    public EncryptionAlgorithm EncryptionAlgorithm { get; set; }

    // max length = 4096
    // Null when the request specified a Recipient
    public byte[] Plaintext { get; init; }

    public byte[] CiphertextForRecipient { get; init; }

    // Omitted when the request specified a Recipient
    public string KeyMaterialId { get; init; }
}