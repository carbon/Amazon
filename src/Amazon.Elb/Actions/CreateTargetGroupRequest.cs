namespace Amazon.Elb
{
    public class CreateTargetGroupRequest : IElbRequest
    {
        public string Action => "CreateTargetGroup";

        public int HealthCheckIntervalSeconds { get; set; }

        public string HealthCheckPath { get; set; }

        // default = traffic-port
        public string HealthCheckPort { get; set; }

        public string HealthCheckProtocal { get; set; }

        public int HealthCheckTimeoutSeconds { get; set; }

        public int HealthyThresholdCount { get; set; }
        
        public Matcher Matcher { get; set; }


        public string Name { get; set; }

        public int Port { get; set; }

        public string Protocal { get; set; }

        public int UnhealthyThresholdCount { get; set; }
        
        public string VpcId { get; set; }

    }
}
