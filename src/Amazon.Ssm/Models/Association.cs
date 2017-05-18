using System;

namespace Amazon.Ssm
{
    public class Association
    {
        public string AssociationId { get; set; }

        public string DocumentVersion { get; set; }

        public string InstanceId { get; set; }

        public DateTime? LastExecutionDate { get; set; }

        public string Name { get; set; }

        public AssociationOverview Overview { get; set; }

        public string ScheduleExpression { get; set;  }

        public Target[] Targets { get; set; }
    }
}