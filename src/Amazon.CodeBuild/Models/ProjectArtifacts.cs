using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class ProjectArtifacts
{
    [JsonPropertyName("type")]
    public required ProjectArtifactsType Type { get; init; }

    [JsonPropertyName("artifactIdentifier")]
    public string? ArtifactIdentifier { get; set; }

    [JsonPropertyName("encryptionDisabled")]
    public bool? EncryptionDisabled { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("namespaceType")]
    public ProjectArtifactsNamespaceType? NamespaceType { get; init; }

    [JsonPropertyName("packaging")]
    public ProjectArtifactsPackagingType? Packaging { get; init; }

    [JsonPropertyName("path")]
    public string? Path { get; init; }
}