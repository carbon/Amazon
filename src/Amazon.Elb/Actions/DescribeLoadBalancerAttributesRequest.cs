namespace Amazon.Elb;

public sealed class DescribeLoadBalancerAttributesRequest : IElbRequest
{
    public DescribeLoadBalancerAttributesRequest(string loadBalancerArn!!)
    {
        LoadBalancerArn = loadBalancerArn;
    }

    public string Action => "DescribeLoadBalancerAttributes";

    public string LoadBalancerArn { get; }
}