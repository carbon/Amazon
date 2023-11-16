using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class PhaseContext
{
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("statusCode")]
    public string? StatusCode { get; init; }
}