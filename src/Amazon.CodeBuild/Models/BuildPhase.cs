using System;

namespace Amazon.CodeBuild
{
    public class BuildPhase
    {
        public PhaseContext[] Contexts { get; set; }

        public long DurationInSeconds { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string PhaseStatus { get; set; }

        public string PhaseType { get; set; }
    }
}
