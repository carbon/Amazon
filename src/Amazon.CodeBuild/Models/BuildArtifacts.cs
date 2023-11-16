using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class BuildArtifacts
{
    [JsonPropertyName("artifactIdentifier")]
    public string? ArtifactIdentifier { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; init; }

    [JsonPropertyName("md5sum")]
    public string? Md5Sum { get; init; }

    [JsonPropertyName("sha256sum")]
    public string? Sha256Sum { get; init; }
}