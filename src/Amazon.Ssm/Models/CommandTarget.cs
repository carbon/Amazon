namespace Amazon.Ssm;

public sealed class CommandTarget
{
    public CommandTarget() { }

    public CommandTarget(string key, params string[] values)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        ArgumentNullException.ThrowIfNull(values);

        Key = key;
        Values = values;
    }

    public required string Key { get; set; }

    public required string[] Values { get; set; }
} 