using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class IpPermission
    {
        // -1
        [XmlElement("ipProtocal")]
        public string IpProtocal { get; set; }
        
        [XmlElement("fromPort")]
        public int FromPort { get; set; }

        [XmlElement("toPort")]
        public string ToPort { get; set; }

        [XmlArray("ipRanges")]
        [XmlArrayItem("item")]
        public IpRange[] IpRanges { get; set; }

        [XmlArray("ipv6Ranges")]
        [XmlArrayItem("item")]
        public Ipv6Range[] Ipv6Ranges { get; set; }

        [XmlArray("groups")]
        [XmlArrayItem("item")]
        public IpPermissionGroup[] Groups { get; set; }
    }
}