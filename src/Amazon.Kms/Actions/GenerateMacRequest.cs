using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class GenerateMacRequest : KmsRequest
{
    public GenerateMacRequest() { }

    [SetsRequiredMembers]
    public GenerateMacRequest(string keyId, MacAlgorithm macAlgorithm, byte[] message)
    {
        ArgumentNullException.ThrowIfNull(message);

        KeyId = keyId;
        MacAlgorithm = macAlgorithm;
        Message = message;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? GrantTokens { get; init; }

    public required string KeyId { get; init; }

    public required MacAlgorithm MacAlgorithm { get; init; }

    public required byte[] Message { get; init; }
 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool DryRun { get; init; }
}