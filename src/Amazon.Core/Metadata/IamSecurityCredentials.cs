#nullable disable

namespace Amazon.Metadata;

internal sealed class IamSecurityCredentials
{
    public string Code { get; init; }

    public string Type { get; init; }

    public string AccessKeyId { get; init; }

    public string SecretAccessKey { get; init; }

    public string Token { get; init; }

    public DateTime LastUpdated { get; init; }

    public DateTime Expiration { get; init; }
}