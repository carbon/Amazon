namespace Amazon.Route53;

public sealed class AliasTarget
{
    public required string DNSName { get; init; }

    public bool EvaluateTargetHealth { get; init; }

    public required string HostedZoneId { get; init; }
}