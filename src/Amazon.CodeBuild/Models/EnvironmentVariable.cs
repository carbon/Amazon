using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }

    // Valid Values: PLAINTEXT | PARAMETER_STORE | SECRETS_MANAGER
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; init; }
}