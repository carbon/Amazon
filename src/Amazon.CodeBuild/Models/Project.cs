#nullable disable

namespace Amazon.CodeBuild;

public class Project
{
    public string Name { get; init; }

    public string Arn { get; init; }

    public ProjectArtifacts[] Artifacts { get; init; }

    public string Description { get; init; }

    public string EncryptionKey { get; init; }

    public ProjectEnvironment Environment { get; init; }

    public Timestamp Created { get; init; }

    public Timestamp LastModified { get; init; }

    public string ServiceRole { get; init; }
    
    public Tag[] Tags { get; init; }

    public int TimeoutInMinutes { get; init; }
}