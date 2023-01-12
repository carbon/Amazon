using System.Diagnostics.CodeAnalysis;

namespace Amazon.Elb;

public sealed class DeregisterTargetsRequest : IElbRequest
{
    public DeregisterTargetsRequest() { }

    [SetsRequiredMembers]
    public DeregisterTargetsRequest(
        string targetGroupArn,
        params TargetDescription[] targets)
    {
        ArgumentException.ThrowIfNullOrEmpty(targetGroupArn);
        ArgumentNullException.ThrowIfNull(targets);

        TargetGroupArn = targetGroupArn;
        Targets = targets;
    }

    public string Action => "DeregisterTargets";

    public required string TargetGroupArn { get; init; }

    public required TargetDescription[] Targets { get; init; }
}