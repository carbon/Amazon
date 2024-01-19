namespace Amazon.Kms;

public sealed class SignResult : KmsResult
{
    public required string KeyId { get; init; }

    public required byte[] Signature { get; init; }

    public required SigningAlgorithm SigningAlgorithm { get; init; }
}