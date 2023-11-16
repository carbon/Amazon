using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ProjectCache
{
    // NO_CACHE | S3
    [JsonPropertyName("type")]
    public required ProjectCacheType Type { get; init; }

    [JsonPropertyName("location")]
    public string? Location { get; init; }

    [JsonPropertyName("modes")]
    public string[]? Modes { get; init; }
}
