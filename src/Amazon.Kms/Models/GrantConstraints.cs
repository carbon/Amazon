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

    // Only allow requests made on behalf of this AWS resource
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceArn { get; init; }
}