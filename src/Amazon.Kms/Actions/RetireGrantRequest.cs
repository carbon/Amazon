using System;

namespace Amazon.Kms;

public sealed class RetireGrantRequest : KmsRequest
{
    public RetireGrantRequest(string grantToken)
    {
        GrantToken = grantToken;
    }

    public RetireGrantRequest(string keyId, string grantId)
    {
        KeyId = keyId ?? throw new ArgumentNullException(nameof(keyId));
        GrantId = grantId ?? throw new ArgumentNullException(nameof(grantId));
    }

    public string? GrantId { get; }

    public string? GrantToken { get; }

    public string? KeyId { get; }
}