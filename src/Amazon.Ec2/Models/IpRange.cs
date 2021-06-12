#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class IpRange
    {
        [XmlElement("cidrIp")]
        public string CidrIp { get; init; }
    }
}