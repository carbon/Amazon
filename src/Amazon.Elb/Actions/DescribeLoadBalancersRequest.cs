namespace Amazon.Elb;

public sealed class DescribeLoadBalancersRequest : IElbRequest
{
    public string Action => "DescribeLoadBalancers";

    public string[]? LoadBalancerArns { get; init; }

    public string? Marker { get; init; }

    public string[]? Names { get; init; }

    public int? PageSize { get; init; }
}
