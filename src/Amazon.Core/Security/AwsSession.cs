namespace Amazon;

public sealed class AwsSession(
    string sessionToken,
    string secretAccessKey,
    DateTime expiration,
    string accessKeyId,
    string? securityToken = null) : IAwsCredential
{
    public string SessionToken { get; } = sessionToken;

    public string SecretAccessKey { get; } = secretAccessKey;

    public string? SecurityToken { get; } = securityToken;

    public string AccessKeyId { get; } = accessKeyId;

    public DateTime Expiration { get; } = expiration;

    public bool ShouldRenew => false;

    public Task<bool> RenewAsync()
    {
        throw new NotImplementedException();
    }
}
