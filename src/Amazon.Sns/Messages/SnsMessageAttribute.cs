namespace Amazon.Sns;

public sealed class SnsMessageAttribute
{
    public required string Type { get; init; }

    public required string Value { get; init; }
}
