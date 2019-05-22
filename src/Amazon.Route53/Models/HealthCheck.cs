#nullable disable


using System.Xml.Serialization;

namespace Amazon.Route53
{
    public class HealthCheck
    {
        public string Id { get; set; }

        public string CallerReference { get; set; }

        public HealthCheckConfig HealthCheckConfig { get; set; }

        public int HealthCheckVersion { get; set; }
    }

    public class HealthCheckConfig
    {
        public string IPAddress { get; set; }

        public int Port { get; set; }

        public string Http { get; set; }

        public string ResourcePath { get; set; }

        public string FullyQualifiedDomainName { get; set; }

        public int RequestInterval { get; set; }

        public int FailureThreshold { get; set; }

        public bool MeasureLatency { get; set; }

        public bool EnableSNI { get; set; }

        [XmlArray]
        [XmlArrayItem("Region")]
        public string[] Regions { get; set; }

        public bool Inverted { get; set; }
    }

}


/*
<HealthCheck>
    <Id>abcdef11-2222-3333-4444-555555fedcba</Id>
    <CallerReference>example.com 192.0.2.17</CallerReference>
    <HealthCheckConfig>
        <IPAddress>192.0.2.17</IPAddress>
        <Port>80</Port>
        <Type>HTTP</Type>
        <ResourcePath>/docs/route-53-health-check.html</ResourcePath>
        <FullyQualifiedDomainName>example.com</FullyQualifiedDomainName>
        <RequestInterval>30</RequestInterval>
        <FailureThreshold>3</FailureThreshold>
        <MeasureLatency>true</MeasureLatency>
        <EnableSNI>true</EnableSNI>
        <Regions>
        <Region>ap-southeast-1</Region>
        <Region>ap-southeast-2</Region>
        <Region>ap-northeast-1</Region>
        </Regions>
        <Inverted>false</Inverted>
    </HealthCheckConfig>
    <HealthCheckVersion>2<HealthCheckVersion>
</HealthCheck>
*/