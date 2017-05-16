using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class StartBuildRequest : ICodeBuildRequest
    {
        public StartBuildRequest(string projectName)
        {
            ProjectName = projectName ?? throw new ArgumentNullException(nameof(projectName));
        }

        public ProjectArtifacts ArtifactsOverride { get; set; }

        public string BuildspecOverride { get; set; }

        public EnvironmentVariable[] EnviromentVariablesOverride { get; set; }

        [Required]
        public string ProjectName { get; set; }

        public string SourceVersion { get; set; }

        public int? TimeoutInMinutesOverride { get; set; }
    }
}