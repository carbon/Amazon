#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb;

public sealed class DeregisterTargetsRequest : IElbRequest
{
    public DeregisterTargetsRequest() { }

    public DeregisterTargetsRequest(
        string targetGroupArn,
        params TargetDescription[] targets)
    {
        ArgumentNullException.ThrowIfNull(targetGroupArn);
        ArgumentNullException.ThrowIfNull(targets);

        TargetGroupArn = targetGroupArn;
        Targets = targets;
    }

    public string Action => "DeregisterTargets";

    [Required]
    public string TargetGroupArn { get; init; }

    [Required]
    public TargetDescription[] Targets { get; init; }
}