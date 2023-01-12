using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

public sealed class BuildArtifacts
{
    public string? Location { get; init; }

    [JsonPropertyName("md5sum")]
    public string? Md5Sum { get; init; }

    [JsonPropertyName("sha256sum")]
    public string? Sha256Sum { get; init; }
}