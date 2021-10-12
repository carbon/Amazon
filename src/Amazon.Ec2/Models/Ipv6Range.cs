#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class Ipv6Range
{
    [XmlElement("cidrIpv6")]
    public string CidrIpv6 { get; init; }
}
