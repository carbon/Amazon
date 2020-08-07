#nullable disable

namespace Amazon.CodeBuild
{
    public class Build
    {
        public string Arn { get; set; }

        public BuildArtifacts Artifacts { get; set; }

        public bool BuildComplete { get; set; }

        // SUCCEEDED | FAILED | FAULT | TIMED_OUT | IN_PROGRESS | STOPPED
        public string BuildStatus { get; set; }

        public ProjectCache Cache { get; set; }

        public string CurrentPhase { get; set; }

        public ProjectEnvironment Environment { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// The entity that started the build.
        /// </summary>
        public string Initiator { get; set; }

        public LogsLocation Logs { get; set; }

        public BuildPhase[] Phases { get; set; }

        public string ProjectName { get; set; }

        // Any version identifier for the version of the source code to be built.
        public string SourceVersion { get; set; }

        public Timestamp StartTime { get; set; }

        public Timestamp? EndTime { get; set; }

        public int? TimeoutInMinutes { get; set; }
    }
}

