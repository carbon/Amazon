namespace Amazon.Sts;

public sealed class AssumeRoleWithSamlRequest : IStsRequest
{
    string IStsRequest.Action => "AssumeRoleWithSAML";

    public int? DurationSeconds { get; init; }

    public string? Policy { get; init; }

    public required string PrincipalArn { get; init; }

    public required string RoleArn { get; init; }

    public required string SAMLAssertion { get; init; }
}