using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? EncryptionContext { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? GrantTokens { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public KeySpec? KeySpec { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? NumberOfBytes { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RecipientInfo? Recipient { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool DryRun { get; init; }
}