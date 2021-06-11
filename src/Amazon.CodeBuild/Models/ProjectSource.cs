#nullable disable

namespace Amazon.CodeBuild
{
    public class ProjectSource
    {
        public string Type { get; init; }

        public SourceAuth Auth { get; init; }

        public string Buildspec { get; init; }

        public string Location { get; init; }
    }
}
