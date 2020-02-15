#nullable disable

using System.Text.Json.Serialization;

using Amazon.CodeBuild.Converters;

namespace Amazon.CodeBuild
{
    public class BuildPhase
    {
        public PhaseContext[] Contexts { get; set; }

        public long DurationInSeconds { get; set; }

        [JsonConverter(typeof(TimestampConverter))]
        public Timestamp StartTime { get; set; }

        [JsonConverter(typeof(NullableTimestampConverter))]
        public Timestamp? EndTime { get; set; }

        public string PhaseStatus { get; set; }

        public string PhaseType { get; set; }
    }
}
