#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class StartBuildResponse
{
    [JsonPropertyName("build")]
    public Build Build { get; init; }
}