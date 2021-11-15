#nullable disable

namespace Amazon.Route53;

public sealed class AliasTarget
{
    public string DNSName { get; init; }

    public bool EvaluateTargetHealth { get; init; }

    public string HostedZoneId { get; init; }
}