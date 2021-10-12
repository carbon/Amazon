namespace Amazon.Ec2;

public readonly struct Filter
{
    public Filter(string name, string value)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Filter(string name, bool value)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value ? "true" : "false";
    }

    public string Name { get; }

    public string Value { get; }
}
