namespace Amazon.Elb;

public sealed class DescribeTargetGroupAttributesRequest : IElbRequest
{
    public string Action => "DescribeTargetGroupAttributes";

    public DescribeTargetGroupAttributesRequest(string targetGroupArn)
    {
        TargetGroupArn = targetGroupArn;
    }

    public string TargetGroupArn { get; }
}
