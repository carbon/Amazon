namespace Amazon.Kms;

public sealed class RetireGrantRequest : KmsRequest
{
    public RetireGrantRequest(string grantToken)
    {
        ArgumentNullException.ThrowIfNull(grantToken);

        GrantToken = grantToken;
    }

    public RetireGrantRequest(string keyId, string grantId)
    {
        ArgumentNullException.ThrowIfNull(keyId);
        ArgumentNullException.ThrowIfNull(grantId);

        KeyId = keyId;
        GrantId = grantId;
    }

    public string? GrantId { get; }

    public string? GrantToken { get; }

    public string? KeyId { get; }
}