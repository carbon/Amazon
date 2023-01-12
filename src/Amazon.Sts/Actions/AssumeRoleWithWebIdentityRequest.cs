namespace Amazon.Sts;

public sealed class AssumeRoleWithWebIdentityRequest : IStsRequest
{
    string IStsRequest.Action => "AssumeRoleWithWebIdentity";

    public int? DurationSeconds { get; init; }

    public string? Policy { get; init; }

    public string? ProviderId { get; init; }

    public required string RoleArn { get; init; }

    public required string RoleSessionName { get; init; }

    public required string WebIdentityToken { get; init; }
}