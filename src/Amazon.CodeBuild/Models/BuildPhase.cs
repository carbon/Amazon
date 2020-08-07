#nullable disable

namespace Amazon.CodeBuild
{
    public class BuildPhase
    {
        public PhaseContext[] Contexts { get; set; }

        public long DurationInSeconds { get; set; }

        public Timestamp StartTime { get; set; }

        public Timestamp? EndTime { get; set; }

        public string PhaseStatus { get; set; }

        public string PhaseType { get; set; }
    }
}