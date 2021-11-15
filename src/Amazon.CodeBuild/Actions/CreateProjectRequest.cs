#nullable disable

namespace Amazon.CodeBuild;

public sealed class CreateProjectRequest : ICodeBuildRequest
{
    public ProjectArtifacts Artifacts { get; set; }

    public string Description { get; set; }

    public string EncryptionKey { get; set; }

    public ProjectEnvironment Environment { get; set; }

    public string Name { get; set; }

    public string ServiceRole { get; set; }

    public ProjectSource Source { get; set; }

    public Tag[] Tags { get; set; }

    public int? TimeoutInMinutes { get; set; }
}
