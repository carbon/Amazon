using System.Text.Json.Serialization;

namespace Amazon.Ssm;

public sealed class CommandFilter
{
    public CommandFilter() { }

    public CommandFilter(string key, string value)
    {
        Key = key;
        Value = value;
    }

    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("value")]
    public required string Value { get; init; }
}
