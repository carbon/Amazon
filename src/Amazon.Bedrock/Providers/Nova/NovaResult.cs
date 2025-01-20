using System.Text.Json.Serialization;

namespace Amazon.Nova;

public sealed partial class NovaResult
{
    [JsonPropertyName("output")]
    public required NovaOutput Output { get; init; }

    [JsonPropertyName("usage")]
    public required NovaUsage Usage { get; init; }
}