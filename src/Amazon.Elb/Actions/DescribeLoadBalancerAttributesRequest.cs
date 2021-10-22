namespace Amazon.Elb;

public sealed class DescribeLoadBalancerAttributesRequest : IElbRequest
{
    public DescribeLoadBalancerAttributesRequest(string loadBalancerArn)
    {
        ArgumentNullException.ThrowIfNull(loadBalancerArn);

        LoadBalancerArn = loadBalancerArn;
    }

    public string Action => "DescribeLoadBalancerAttributes";

    public string LoadBalancerArn { get; }
}