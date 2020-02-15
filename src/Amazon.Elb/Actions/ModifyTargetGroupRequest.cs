#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public sealed class ModifyTargetGroupRequest : IElbRequest
    {
        public string Action => "ModifyTargetGroup";

        public int? HealthCheckIntervalSeconds { get; set; }

        public string HealthCheckPath { get; set; }

        public int? HealthCheckPort { get; set; }

        public string HealthCheckProtocol { get; set; }

        public int? HealthCheckTimeoutSeconds { get; set; }

        public int? HealthyThresholdCount { get; set; }

        public Matcher Matcher { get; set; }

        [Required]
        public string TargetGroupArn { get; set; }

        public int? UnhealthyThresholdCount { get; set; }
    }
}