namespace Amazon.Ec2;

public readonly struct Filter
{
    public Filter(string name, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(value);

        Name = name;
        Value = value;
    }

    public Filter(string name, bool value)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
        Value = value ? "true" : "false";
    }

    public string Name { get; }

    public string Value { get; }
}