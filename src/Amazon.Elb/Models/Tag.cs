#nullable disable

using System.Diagnostics.CodeAnalysis;

namespace Amazon.Elb;

public sealed class Tag
{
    public Tag() { }

    [SetsRequiredMembers]
    public Tag(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public required string Key { get; init; }

    public string Value { get; init; }
}