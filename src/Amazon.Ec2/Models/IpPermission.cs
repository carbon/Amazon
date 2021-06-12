#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class IpPermission
    {
        // tcp | udp | icmp | icmpv6
        [XmlElement("ipProtocol")]
        public string IpProtocol { get; init; }
        
        [XmlElement("fromPort")]
        public int FromPort { get; init; }

        [XmlElement("toPort")]
        public string ToPort { get; init; }

        [XmlArray("ipRanges")]
        [XmlArrayItem("item")]
        public IpRange[] IpRanges { get; init; }

        [XmlArray("ipv6Ranges")]
        [XmlArrayItem("item")]
        public Ipv6Range[] Ipv6Ranges { get; init; }

        [XmlArray("groups")]
        [XmlArrayItem("item")]
        public IpPermissionGroup[] Groups { get; init; }
    }
}