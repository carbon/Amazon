using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class EnvironmentPlatform
{
    /// <summary>
    /// The list of programming languages that are available for the specified platform.
    /// </summary>
    [JsonPropertyName("languages")]
    public EnvironmentLanguage[]? Languages { get; init; }

    // DEBIAN | AMAZON_LINUX | UBUNTU
    [JsonPropertyName("platform")]
    public string? Platform { get; init; }
}