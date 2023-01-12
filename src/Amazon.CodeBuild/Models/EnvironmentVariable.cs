using System.Diagnostics.CodeAnalysis;

namespace Amazon.CodeBuild;

public sealed class EnvironmentVariable
{
    public EnvironmentVariable() { }

    [SetsRequiredMembers]
    public EnvironmentVariable(string name, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(value);

        Name = name;
        Value = value;
    }

    public required string Name { get; init; }

    public required string Value { get; init; }
}