using System.Text.Json.Serialization;

using Amazon.Ssm.Converters;

namespace Amazon.Ssm
{
    public sealed class Association
    {
        public string AssociationId { get; set; }

        public string DocumentVersion { get; set; }

        public string InstanceId { get; set; }

        [JsonConverter(typeof(NullableTimestampConverter))]
        public Timestamp? LastExecutionDate { get; set; }

        public string Name { get; set; }

        public AssociationOverview Overview { get; set; }

        public string ScheduleExpression { get; set;  }

        public Target[] Targets { get; set; }
    }
}