using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class RevokeGrantRequest(string keyId, string grantId) : KmsRequest
{
    public string KeyId { get; } = keyId ?? throw new ArgumentNullException(nameof(keyId));

    public string GrantId { get; } = grantId ?? throw new ArgumentNullException(nameof(keyId));

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool DryRun { get; init; }
}