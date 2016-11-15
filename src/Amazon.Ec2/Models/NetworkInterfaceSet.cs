using System.Collections.Generic;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class NetworkInterfaceSet
    {
        [XmlElement("item")]
        public List<NetworkInterface> Items { get; set; }
    }
}
