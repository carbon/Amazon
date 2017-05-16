namespace Amazon.CodeBuild
{
    public class Build
    {
        public string Arn { get; set; }

        public BuildArtifacts[] Artifacts { get; set; }

        public bool BuildComplete { get; set; }

        public string BuildStatus { get; set; }

        public string CurrentPhase { get; set; }

        public ProjectEnvironment Environment { get; set; }

        public string Id { get; set; }

        public string Initiator { get; set; }

        public LogsLocation Logs { get; set; }

        public BuildPhase Phases { get; set; }

        public string ProjectName { get; set; }

        // Any version identifier for the version of the source code to be built.
        public string SourceVersion { get; set; }

        public string StartTime { get; set; }

        public int TimeoutInMinutes { get; set; }
    }
}
