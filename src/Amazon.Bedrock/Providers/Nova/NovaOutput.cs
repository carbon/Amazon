using System.Text.Json.Serialization;

namespace Amazon.Nova;

public sealed class NovaOutput
{
    [JsonPropertyName("message")]
    public required NovaMessage Message { get; init; }
}