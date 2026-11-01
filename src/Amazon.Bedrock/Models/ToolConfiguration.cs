using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class ToolConfiguration
{
    [JsonPropertyName("toolChoice")]
    public JsonElement? ToolChoice { get; init; }

    [JsonPropertyName("tools")]
    public required List<JsonElement> Tools { get; init; }
}