namespace Amazon.CodeBuild;

public sealed class EnvironmentVariable
{
    public EnvironmentVariable() { }

    public EnvironmentVariable(string name, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNull(value);

        Name = name;
        Value = value;
    }

    public required string Name { get; init; }

    public required string Value { get; init; }
}