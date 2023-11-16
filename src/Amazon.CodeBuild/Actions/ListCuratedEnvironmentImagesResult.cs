using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ListCuratedEnvironmentImagesResult
{
    [JsonPropertyName("platforms")]
    public required EnvironmentPlatform Platforms { get; init; }
}