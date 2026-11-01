namespace Amazon.CloudWatch;

public readonly struct AlarmTag
{
    public required string Key { get; init; }

    public required string Value { get; init; }
}
