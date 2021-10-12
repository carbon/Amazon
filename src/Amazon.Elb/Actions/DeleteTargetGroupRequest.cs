namespace Amazon.Elb;

public sealed class DeleteTargetGroupRequest : IElbRequest
{
    public string Action => "DeleteTargetGroup";

    public DeleteTargetGroupRequest(string targetGroupArn)
    {
        TargetGroupArn = targetGroupArn ?? throw new ArgumentNullException(nameof(targetGroupArn));
    }

    public string TargetGroupArn { get; }
}