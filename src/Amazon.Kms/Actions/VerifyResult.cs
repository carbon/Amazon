namespace Amazon.Kms;

public sealed class VerifyResult : KmsResult
{
    public required string KeyId { get; init; }

    public required bool SignatureValid { get; init; }

    public required SigningAlgorithm SigningAlgorithm { get; init; }
}