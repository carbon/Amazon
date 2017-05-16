namespace Amazon.CodeBuild
{
    public class ProjectSource
    {
        public string Type { get; set; }

        public SourceAuth Auth { get; set; }

        public string Buildspec { get; set; }

        public string Location { get; set; }
    }
}
