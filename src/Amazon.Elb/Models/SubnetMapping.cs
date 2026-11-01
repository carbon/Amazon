namespace Amazon.Elb;

public sealed class SubnetMapping
{
    public string? AllocationId { get; init; }

    public string? IPv6Address { get; init; }

    public string? PrivateIPv4Address { get; init; }

    public string? SourceNatIpv6Prefix { get; init; }

    public string? SubnetId { get; init; }
}
