namespace Amazon.Ec2;

public readonly struct Filter
{
    public Filter(string name, string value)
    {
#if NET7_0_OR_GREATER
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(value);
#else
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(value);
#endif

        Name = name;
        Value = value;
    }

    public Filter(string name, bool value)
    {
#if NET7_0_OR_GREATER
        ArgumentException.ThrowIfNullOrEmpty(name);
#else
        ArgumentNullException.ThrowIfNull(name);
#endif

        Name = name;
        Value = value ? "true" : "false";
    }

    public string Name { get; }

    public string Value { get; }
}