using System;

namespace Amazon.DynamoDb.Models;

public sealed class Tag
{
#nullable disable
    public Tag() { }
#nullable enable

    public Tag(string key, string value)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        Key = key;
        Value = value;
    }

    public string Key { get; init; }

    public string Value { get; init; }
}
