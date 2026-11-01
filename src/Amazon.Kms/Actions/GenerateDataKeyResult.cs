#nullable disable

namespace Amazon.Kms;

public sealed class GenerateDataKeyResult : KmsResult
{
    public required string KeyId { get; init; }

    public byte[] CiphertextBlob { get; init; }

    // Null when the request specified a Recipient
    public byte[] Plaintext { get; init; }

    public byte[] CiphertextForRecipient { get; init; }
}