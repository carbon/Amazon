using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class RetireGrantRequest : KmsRequest
{
    public RetireGrantRequest(string grantToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(grantToken);

        GrantToken = grantToken;
    }

    public RetireGrantRequest(string keyId, string grantId)
    {
        ArgumentException.ThrowIfNullOrEmpty(keyId);
        ArgumentException.ThrowIfNullOrEmpty(grantId);

        KeyId = keyId;
        GrantId = grantId;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GrantId { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GrantToken { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? KeyId { get; }
}