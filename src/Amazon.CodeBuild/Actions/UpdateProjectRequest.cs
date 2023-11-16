using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class UpdateProjectRequest : ICodeBuildRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("artifacts")]
    public ProjectArtifacts? Artifacts { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("encryptionKey")]
    public string? EncryptionKey { get; set; }

    [JsonPropertyName("environment")]
    public ProjectEnvironment? Environment { get; set; }

    [JsonPropertyName("serviceRole")]
    public string? ServiceRole { get; set; }

    [JsonPropertyName("source")]
    public ProjectSource? Source { get; set; }

    [JsonPropertyName("tags")]
    public Tag[]? Tags { get; set; }

    [JsonPropertyName("timeoutInMinutes")]
    public int? TimeoutInMinutes { get; set; }
}