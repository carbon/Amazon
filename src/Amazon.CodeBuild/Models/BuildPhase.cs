
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class BuildPhase
    {
        public PhaseContext[] Contexts { get; set; }

        public long DurationInSeconds { get; set; }

        public string EndTime { get; set; }

        public string PhaseStatus { get; set; }

        public string PhaseType { get; set; }

        public string StartTime { get; set; }
    }
}
