#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public class LoadBalancer
{
    [XmlElement]
    public string LoadBalancerArn { get; init; }

    [XmlElement]
    public string Scheme { get; init; }

    [XmlElement]
    public string LoadBalancerName { get; init; }

    [XmlElement]
    public string VpcId { get; init; }

    [XmlElement]
    public string CanonicalHostedZoneId { get; init; }

    [XmlElement]
    public DateTime CreateTime { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public AvailabilityZone[] AvailabilityZones { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public string[] SecurityGroups { get; init; }

    [XmlElement]
    public string DNSName { get; init; }

    [XmlElement]
    public LoadBalancerState State { get; init; }

    [XmlElement]
    public string Type { get; init; }
}

public sealed class LoadBalancerState
{
    //  active | provisioning | failed
    [XmlElement]
    public string Code { get; init; }

    [XmlElement]
    public string Reason { get; init; }
}
