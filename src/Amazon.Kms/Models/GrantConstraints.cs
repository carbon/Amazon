using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class GrantConstraints
{
    // Match all
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? EncryptionContextEquals { get; init; }

    // Match any
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? EncryptionContextSubset { get; init; }
}