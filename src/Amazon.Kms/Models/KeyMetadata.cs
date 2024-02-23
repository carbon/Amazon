namespace Amazon.Kms;

public sealed class KeyMetadata
{
    public required string KeyId { get; init; }

    public string? Arn { get; init; }

    public string? AWSAccountId { get; init; }

    public string? CloudHsmClusterId { get; init; }

    public string? CustomKeyStoreId { get; init; }

    public string? Description { get; init; }

    public bool Enabled { get; init; }

    public EncryptionAlgorithm[]? EncryptionAlgorithms { get; init; }

    public string? ExpirationModel { get; init; }

    public KeyManager? KeyManager { get; init; }

    public KeySpec KeySpec { get; init; }

    public KeyState KeyState { get; init; }

    public bool MultiRegion { get; init; }

    public string? Origin { get; init; }
}