using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesError
{
    [JsonPropertyName("message")]
    public required string? Message { get; init; }
}
