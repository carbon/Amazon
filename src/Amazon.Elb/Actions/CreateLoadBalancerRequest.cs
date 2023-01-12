using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Amazon.Elb;

public sealed class CreateLoadBalancerRequest : IElbRequest
{
    public CreateLoadBalancerRequest() { }

    [SetsRequiredMembers]
    public CreateLoadBalancerRequest(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
    }

    public string Action => "CreateLoadBalancer";

    public string? IpAddressType { get; init; }

    [MaxLength(32)]
    public required string Name { get; init; }

    public string? Scheme { get; init; }

    public string[]? SecurityGroups { get; init; }

    // Must specifiy at least 2 subnets
    [Required]
    public string[]? Subnets { get; init; }

    public Tag[]? Tags { get; init; }
}
