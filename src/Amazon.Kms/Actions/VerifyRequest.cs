using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class VerifyRequest : KmsRequest
{
    public VerifyRequest() { }

    [SetsRequiredMembers]
    public VerifyRequest(string keyId, byte[] message, byte[] signature, SigningAlgorithm signingAlgorithm)
    {
        KeyId = keyId;
        Message = message;
        Signature = signature;
        SigningAlgorithm = signingAlgorithm;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? GrantTokens { get; init; }

    public required string KeyId { get; init; }

    public required byte[] Message { get; init; }

    // RAW | DIGEST
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MessageType { get; init; }

    public required byte[] Signature { get; init; }

    public required SigningAlgorithm SigningAlgorithm { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool DryRun { get; init; }
}