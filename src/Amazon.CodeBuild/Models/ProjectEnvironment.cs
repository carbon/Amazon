#nullable disable

namespace Amazon.CodeBuild
{
    public class ProjectEnvironment
    {
        public string ComputeType { get; set; }

        public string Image { get; set; }

        public string Type { get; set; }

        public EnvironmentVariable[] EnvironmentVariables { get; set; }
    }
}
