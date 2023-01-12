using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class Target
{
    public Target() { }

    public Target(string key, string[] values)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(values);

        Key = key;
        Values = values;
    }

    [StringLength(128)]
    public required string Key { get; init; }

    public required string[] Values { get; init; }
}