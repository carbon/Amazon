using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class Tag
{
    public Tag() { }

    [SetsRequiredMembers]
    public Tag(string key, string value)
    {
        Key = key;
        Value = value;
    }

    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }
}