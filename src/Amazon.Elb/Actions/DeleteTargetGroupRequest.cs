namespace Amazon.Elb;

public sealed class DeleteTargetGroupRequest : IElbRequest
{
    public string Action => "DeleteTargetGroup";

    public DeleteTargetGroupRequest(string targetGroupArn)
    {
        ArgumentNullException.ThrowIfNull(targetGroupArn);

        TargetGroupArn = targetGroupArn;
    }

    public string TargetGroupArn { get; }
}