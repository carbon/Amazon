using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class EnvironmentLanguage
{
    [JsonPropertyName("images")]
    public EnvironmentImage[]? Images { get; init; }

    [JsonPropertyName("language")]
    public Language Language { get; init; }
}
