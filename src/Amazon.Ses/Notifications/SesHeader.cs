#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

[method: JsonConstructor]
public readonly struct SesHeader(string name, string value)
{
    [JsonPropertyName("name")]
    public string Name { get; } = name;

    [JsonPropertyName("value")]
    public string Value { get; } = value;
}
