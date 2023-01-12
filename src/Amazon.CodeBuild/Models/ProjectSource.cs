using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ProjectSource
{
    [JsonPropertyName("type")]
    public required ProjectSourceType Type { get; init; }

    [JsonPropertyName("auth")]
    public SourceAuth? Auth { get; init; }

    [JsonPropertyName("buildspec")]
    public string? Buildspec { get; init; }

    [JsonPropertyName("location")]
    public string? Location { get; init; }

    [JsonPropertyName("gitCloneDepth")]
    public int? GitCloneDepth { get; set; }
}
