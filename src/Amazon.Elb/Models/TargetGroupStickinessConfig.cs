namespace Amazon.Elb;

public sealed class TargetGroupStickinessConfig
{
    public int? DurationSeconds { get; init; }

    public bool? Enabled { get; init; }
}
