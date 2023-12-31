using System.ComponentModel.DataAnnotations;

namespace Amazon.Kms;

public sealed class GenerateDataKeyWithoutPlaintextResult : KmsResult
{
    public required string KeyId { get; init; }

    [MaxLength(6144)]
    public required byte[] CiphertextBlob { get; init; }
}