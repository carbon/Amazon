using System.Diagnostics.CodeAnalysis;

namespace Amazon.Elb;

public sealed class RegisterTargetsRequest : IElbRequest
{
    public RegisterTargetsRequest() { }

    [SetsRequiredMembers]
    public RegisterTargetsRequest(string targetGroupArn, params TargetDescription[] targets)
    {
        ArgumentException.ThrowIfNullOrEmpty(targetGroupArn);
        ArgumentNullException.ThrowIfNull(targets);

        TargetGroupArn = targetGroupArn;
        Targets = targets;
    }

    public string Action => "RegisterTargets";

    public required string TargetGroupArn { get; init; }

    public required TargetDescription[] Targets { get; init; }
}