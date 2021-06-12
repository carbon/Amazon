#nullable disable

namespace Amazon.Elb
{
    public sealed class CreateTargetGroupRequest : IElbRequest
    {
        public string Action => "CreateTargetGroup";

        public int HealthCheckIntervalSeconds { get; init; }

        public string HealthCheckPath { get; init; }

        // default = traffic-port
        public string HealthCheckPort { get; init; }

        public string HealthCheckProtocol { get; init; }

        public int HealthCheckTimeoutSeconds { get; init; }

        public int HealthyThresholdCount { get; init; }
        
        public Matcher Matcher { get; init; }

        public string Name { get; init; }

        public int Port { get; init; }

        public string Protocol { get; init; }

        public int UnhealthyThresholdCount { get; init; }
        
        public string VpcId { get; init; }
    }
}
