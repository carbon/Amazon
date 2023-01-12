namespace Amazon.CodeBuild;

public sealed class CreateProjectRequest : ICodeBuildRequest
{
    public required ProjectArtifacts Artifacts { get; set; }

    public required ProjectEnvironment Environment { get; set; }

    public string? Description { get; set; }

    public string? EncryptionKey { get; set; }

    public required string Name { get; set; }

    public required string ServiceRole { get; set; }

    public required ProjectSource Source { get; set; }

    public Tag[]? Tags { get; set; }

    public bool? BadgeEnabled { get; set; }

    public int? TimeoutInMinutes { get; set; }
}