namespace Amazon.Ssm;

public sealed class CommandTarget
{
#nullable disable
    public CommandTarget() { }
#nullable enable

    public CommandTarget(string key, params string[] values)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(values);

        Key = key;
        Values = values;
    }

    public string Key { get; set; }

    public string[] Values { get; set; }
} 