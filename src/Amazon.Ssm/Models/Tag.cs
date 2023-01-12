namespace Amazon.Ssm;

public sealed class Tag
{
    public Tag() { }

    public Tag(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public required string Key { get; init; }

    public required string Value { get; init; }
}