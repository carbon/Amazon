#nullable disable

namespace Amazon.Sts;

public sealed class AssumeRoleWithWebIdentityRequest : IStsRequest
{
    string IStsRequest.Action => "AssumeRoleWithWebIdentity";

    public int DurationSeconds { get; init; }

    public string Policy { get; init; }

    public string ProviderId { get; init; }

    public string RoleArn { get; init; }

    public string RoleSessionName { get; init; }

    public string WebIdentityToken { get; init; }
}