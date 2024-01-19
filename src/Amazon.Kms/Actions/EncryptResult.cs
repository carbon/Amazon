namespace Amazon.Kms;

public sealed class EncryptResult : KmsResult
{
    public required byte[] CiphertextBlob { get; init; }

    public EncryptionAlgorithm EncryptionAlgorithm { get; set; }

    public required string KeyId { get; init; }
}