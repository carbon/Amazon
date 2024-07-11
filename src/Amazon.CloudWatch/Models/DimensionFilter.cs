namespace Amazon.CloudWatch;

public readonly struct DimensionFilter(string name, string value)
{
    public string Name { get; } = name;

    public string Value { get; } = value;
}