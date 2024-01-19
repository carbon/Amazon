namespace Amazon.Kms;

public sealed class DecryptResult : KmsResult
{
    public required string KeyId { get; init; }

    public EncryptionAlgorithm EncryptionAlgorithm { get; set; }

    // max length = 4096
    public required byte[] Plaintext { get; init; }
}