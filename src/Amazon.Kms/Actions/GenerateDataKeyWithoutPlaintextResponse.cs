#nullable disable

namespace Amazon.Kms;

public sealed class GenerateDataKeyWithoutPlaintextResponse : KmsResponse
{
    public string KeyId { get; init; }

    public byte[] CiphertextBlob { get; init; }
}