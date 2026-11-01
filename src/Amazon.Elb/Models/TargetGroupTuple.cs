namespace Amazon.Elb;

public sealed class TargetGroupTuple
{
    public string? TargetGroupArn { get; init; }

    public int? Weight { get; init; }
}
