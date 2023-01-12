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

    public string? GrantId { get; }

    public string? GrantToken { get; }

    public string? KeyId { get; }
}