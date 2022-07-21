namespace Amazon.Ec2;

public readonly struct Filter
{
    public Filter(string name, string value)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(value);

        Name = name;
        Value = value;
    }

    public Filter(string name, bool value)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        Value = value ? "true" : "false";
    }

    public string Name { get; }

    public string Value { get; }
}