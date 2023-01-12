using System.Diagnostics.CodeAnalysis;

namespace Amazon.CodeBuild;

public sealed class StartBuildRequest : ICodeBuildRequest
{
    public StartBuildRequest() { }

    [SetsRequiredMembers]
    public StartBuildRequest(string projectName)
    {
        ArgumentException.ThrowIfNullOrEmpty(projectName);

        ProjectName = projectName;
    }

    public required string ProjectName { get; set; }

    public ProjectArtifacts? ArtifactsOverride { get; set; }

    public string? BuildspecOverride { get; set; }

    public EnvironmentVariable[]? EnvironmentVariablesOverride { get; set; }

    public string? SourceVersion { get; set; }

    public int? TimeoutInMinutesOverride { get; set; }
}