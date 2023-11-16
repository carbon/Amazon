using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class EnvironmentImage
{
    [JsonPropertyName("description")]
    public string? Description { get; init; }

#nullable disable

    /// <summary>
    /// The name of the docker image
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }
}