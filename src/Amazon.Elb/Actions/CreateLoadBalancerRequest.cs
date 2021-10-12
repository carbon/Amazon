#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb;

public sealed class CreateLoadBalancerRequest : IElbRequest
{
    public CreateLoadBalancerRequest() { }

    public CreateLoadBalancerRequest(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Action => "CreateLoadBalancer";

    public string IpAddressType { get; init; }

    [Required, MaxLength(32)]
    public string Name { get; init; }

    public string Scheme { get; init; }

    public string[] SecurityGroups { get; init; }

    // Must specifiy at least 2 subnets
    [Required]
    public string[] Subnets { get; init; }

    public Tag[] Tags { get; init; }
}
