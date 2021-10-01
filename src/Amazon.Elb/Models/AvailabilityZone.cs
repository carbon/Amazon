#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class AvailabilityZone
{
    [XmlElement]
    public string SubnetId { get; init; }

    [XmlElement]
    public string ZoneName { get; init; }
}