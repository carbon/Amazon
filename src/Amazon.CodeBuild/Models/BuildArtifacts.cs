#nullable disable

namespace Amazon.CodeBuild;

public sealed class BuildArtifacts
{
    public string Location { get; init; }

    public string Md5Sum { get; init; }

    public string Sha256Sum { get; init; }
}