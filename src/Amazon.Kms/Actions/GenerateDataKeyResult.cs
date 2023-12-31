#nullable disable

namespace Amazon.Kms;

public sealed class GenerateDataKeyResult : KmsResult
{
    public required string KeyId { get; init; }

    public byte[] CiphertextBlob { get; init; }

    public byte[] Plaintext { get; init; }
}