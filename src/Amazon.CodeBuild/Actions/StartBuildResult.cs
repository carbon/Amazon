using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class StartBuildResult
{
    [JsonPropertyName("build")]
    public required Build Build { get; init; }
}