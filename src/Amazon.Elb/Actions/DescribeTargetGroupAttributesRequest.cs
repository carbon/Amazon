namespace Amazon.Elb;

public sealed class DescribeTargetGroupAttributesRequest(string targetGroupArn) : IElbRequest
{
    public string Action => "DescribeTargetGroupAttributes";

    public string TargetGroupArn { get; } = targetGroupArn ?? throw new ArgumentNullException(nameof(targetGroupArn));
}
