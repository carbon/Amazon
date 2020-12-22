#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class TargetGroup
    {
        [XmlElement]
        public string TargetGroupArn { get; set; }

        [XmlElement]
        public int HealthCheckIntervalSeconds { get; set; }

        [XmlElement]
        public string HealthCheckPath { get; set; }

        // default = traffic-port
        [XmlElement]
        public string HealthCheckPort { get; set; }

        [XmlElement]
        public string HealthCheckProtocal { get; set; }

        [XmlElement]
        public int HealthCheckTimeoutSeconds { get; set; }

        [XmlElement]
        public int HealthyThresholdCount { get; set; }

        [XmlElement]
        public Matcher Matcher { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public int Port { get; set; }

        [XmlElement]
        public string Protocal { get; set; }

        [XmlElement]
        public int UnhealthyThresholdCount { get; set; }

        [XmlElement]
        public string VpcId { get; set; }
    }
}

/*
<member> 
    <TargetGroupArn>arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067</TargetGroupArn> 
    <HealthCheckTimeoutSeconds>5</HealthCheckTimeoutSeconds> 
    <HealthCheckPort>traffic-port</HealthCheckPort> 
    <Matcher>
        <HttpCode>200</HttpCode> 
    </Matcher> 
    <TargetGroupName>my-targets</TargetGroupName> 
    <HealthCheckProtocol>HTTP</HealthCheckProtocol> 
    <HealthCheckPath>/</HealthCheckPath> 
    <Protocol>HTTP</Protocol> 
    <Port>80</Port> 
    <VpcId>vpc-3ac0fb5f</VpcId> 
    <HealthyThresholdCount>5</HealthyThresholdCount> 
    <HealthCheckIntervalSeconds>30</HealthCheckIntervalSeconds> 
    <UnhealthyThresholdCount>2</UnhealthyThresholdCount> 
</member> 
*/