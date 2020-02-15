using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public sealed class StartBuildRequest : ICodeBuildRequest
    {
#nullable disable
        public StartBuildRequest() { }
#nullable enable

        public StartBuildRequest(string projectName)
        {
            ProjectName = projectName ?? throw new ArgumentNullException(nameof(projectName));
        }
        
        [Required]
        public string ProjectName { get; set; }

        public ProjectArtifacts? ArtifactsOverride { get; set; }

        public string? BuildspecOverride { get; set; }

        public EnvironmentVariable[]? EnvironmentVariablesOverride { get; set; }

        public string? SourceVersion { get; set; }

        public int? TimeoutInMinutesOverride { get; set; }
    }
}