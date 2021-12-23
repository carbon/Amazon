#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public readonly struct SesHeader
{
    [JsonConstructor]
    public SesHeader(string name, string value)
    {
        Name = name;
        Value = value;
    }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("value")]
    public string Value { get; }
}
