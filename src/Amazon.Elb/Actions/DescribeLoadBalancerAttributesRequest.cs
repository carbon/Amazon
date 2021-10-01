using System;

namespace Amazon.Elb;

public sealed class DescribeLoadBalancerAttributesRequest : IElbRequest
{
    public DescribeLoadBalancerAttributesRequest(string loadBalancerArn)
    {
        LoadBalancerArn = loadBalancerArn ?? throw new ArgumentNullException(nameof(loadBalancerArn));
    }

    public string Action => "DescribeLoadBalancerAttributes";

    public string LoadBalancerArn { get; }
}