#nullable disable

namespace Amazon.CodeBuild
{
    public sealed class ProjectEnvironment
    {
        public string ComputeType { get; init; }

        public string Image { get; init; }

        public string Type { get; init; }

        public EnvironmentVariable[] EnvironmentVariables { get; init; }
    }
}
