namespace Amazon.Kms;

public sealed class GenerateDataKeyWithoutPlaintextResult : KmsResult
{
    public required string KeyId { get; init; }

    // max-length: 6144
    public required byte[] CiphertextBlob { get; init; }
}