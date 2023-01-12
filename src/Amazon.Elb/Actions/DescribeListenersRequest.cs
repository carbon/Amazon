namespace Amazon.Elb;

public sealed class DescribeListenersRequest : IElbRequest
{
    public string Action => "DescribeListeners";

    public string[]? ListenerArns { get; init; }

    public string? LoadBalancerArn { get; init; }

    public string? Marker { get; init; }

    public int? PageSize { get; init; }
}