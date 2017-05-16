using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class Ipv6CidrBlockState
    {
        [XmlElement("state")]
        public string State { get; set; }
    }
}