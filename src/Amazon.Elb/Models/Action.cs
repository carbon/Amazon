#nullable disable

using System.Diagnostics.CodeAnalysis;

namespace Amazon.Elb;

public sealed class Action
{
    public Action() { }

    [SetsRequiredMembers]
    public Action(string targetGroupArn, string type = "forward")
    {
        TargetGroupArn = targetGroupArn;
        Type = type;
    }

    public string TargetGroupArn { get; init; }

    // forward
    public required string Type { get; init; }
}