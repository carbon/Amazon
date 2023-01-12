namespace Amazon.Elb;

public sealed class ModifyLoadBalancerAttributesRequest : IElbRequest
{
    public string Action => "ModifyLoadBalancerAttributes";

    public required LoadBalancerAttribute[] Attributes { get; init; }

    public required string LoadBalancerArn { get; init; }
}