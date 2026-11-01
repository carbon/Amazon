namespace Amazon.Elb;

public sealed class SetSubnetsRequest : IElbRequest
{
    public string Action => "SetSubnets";

    public required string LoadBalancerArn { get; init; }

    public string[]? Subnets { get; init; }
}