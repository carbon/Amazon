using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class NetworkInterfaceSet
    {
        [XmlElement("item")]
        public NetworkInterface[] Items { get; set; }
    }
}