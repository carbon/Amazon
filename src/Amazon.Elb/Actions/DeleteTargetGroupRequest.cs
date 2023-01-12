namespace Amazon.Elb;

public sealed class DeleteTargetGroupRequest : IElbRequest
{
    public DeleteTargetGroupRequest(string targetGroupArn)
    {
        ArgumentException.ThrowIfNullOrEmpty(targetGroupArn);

        TargetGroupArn = targetGroupArn;
    }

    public string Action => "DeleteTargetGroup";

    public string TargetGroupArn { get; }
}