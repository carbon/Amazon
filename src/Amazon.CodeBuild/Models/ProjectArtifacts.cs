#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild;

public sealed class ProjectArtifacts
{
    public string Location { get; init; }

    public string Name { get; init; }

    // NONE | BUILD_ID
    public string NamespaceType { get; init; }

    public string Packaging { get; init; }

    public string Path { get; init; }

    [Required]
    public string Type { get; init; }
}