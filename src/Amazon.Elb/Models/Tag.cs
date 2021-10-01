#nullable disable

namespace Amazon.Elb;

public sealed class Tag
{
    public Tag() { }

    public Tag(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; init; }

    public string Value { get; init; }
}