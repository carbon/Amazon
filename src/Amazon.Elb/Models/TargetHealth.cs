namespace Amazon.Elb;

public sealed class TargetHealth
{
    public string? Description { get; init; }

    public string? Reason { get; init; }

    // initial | healthy | unhealthy | unhealthy.draining | unused | draining | unavailable
    public string? State { get; init; }
}