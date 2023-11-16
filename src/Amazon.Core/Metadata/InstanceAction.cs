#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Metadata;

public sealed class InstanceAction
{
    [JsonPropertyName("action")]
    public string Action { get; init; }

    [JsonPropertyName("time")]
    public DateTime Time { get; init; }
}