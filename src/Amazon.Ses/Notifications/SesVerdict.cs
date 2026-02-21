using System.Text.Json.Serialization;

namespace Amazon.Ses;

public readonly struct SesVerdict
{
    [JsonPropertyName("status")]
    public required string Status { get; init; }
}