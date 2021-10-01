#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class TargetGroup
{
    [XmlElement]
    public string TargetGroupArn { get; init; }

    [XmlElement]
    public bool HealthCheckEnabled { get; init; }

    [XmlElement]
    public int HealthCheckIntervalSeconds { get; init; }

    [StringLength(1_024)]
    [XmlElement]
    public string HealthCheckPath { get; init; }

    // default = traffic-port
    [XmlElement]
    public string HealthCheckPort { get; init; }

    // HTTP | HTTPS | TCP | TLS | UDP | TCP_UDP | GENEVE
    [XmlElement]
    public string HealthCheckProtocol { get; init; }

    [Range(2, 120)]
    [XmlElement]
    public int HealthCheckTimeoutSeconds { get; init; }

    [Range(2, 10)]
    [XmlElement]
    public int HealthyThresholdCount { get; init; }

    [XmlElement]
    public Matcher Matcher { get; init; }

    [XmlElement]
    public string Name { get; init; }

    [XmlElement]
    public int Port { get; init; }

    [XmlElement]
    public string Protocol { get; init; }

    // GRPC | HTTP1 | HTTP2
    [XmlElement]
    public string ProtocolVersion { get; init; }

    [XmlElement]
    public string TargetGroupName { get; init; }

    [XmlElement]
    public string TargetType { get; init; }

    [XmlElement]
    public int UnhealthyThresholdCount { get; init; }

    [XmlElement]
    public string VpcId { get; init; }
}
