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

    public string? TargetGroupArn { get; init; }

    // forward | authenticate-oidc | authenticate-cognito | redirect | fixed-response
    public required string Type { get; init; }
}