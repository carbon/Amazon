using System.Diagnostics.CodeAnalysis;

namespace Amazon.Kms;

public sealed class GenerateDataKeyRequest : KmsRequest
{
    public GenerateDataKeyRequest() { }

    [SetsRequiredMembers]
    public GenerateDataKeyRequest(
        string keyId,
        KeySpec keySpec,
        IReadOnlyDictionary<string, string>? encryptionContext)
    {
        ArgumentException.ThrowIfNullOrEmpty(keyId);

        KeyId = keyId;
        KeySpec = keySpec;
        EncryptionContext = encryptionContext;
    }

    public required string KeyId { get; init; }

    public IReadOnlyDictionary<string, string>? EncryptionContext { get; init; }

    public string[]? GrantTokens { get; init; }

    public KeySpec? KeySpec { get; init; }

    public int? NumberOfBytes { get; init; }
}