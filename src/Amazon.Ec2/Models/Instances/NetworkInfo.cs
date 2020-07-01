#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class NetworkInfo
    {
        [XmlElement("enaSupport")]
        public string EnaSupport { get; set; }

        [XmlElement("ipv4AddressesPerInterface")]
        public int Ipv4AddressesPerInterface { get; set; }

        [XmlElement("ipv6AddressesPerInterface")]
        public int Ipv6AddressesPerInterface { get; set; }

        [XmlElement("ipv6Supported")]
        public bool Ipv6Supported { get; set; }

        [XmlElement("maximumNetworkInterfaces")]
        public int MaximumNetworkInterfaces { get; set; }

        [XmlElement("networkPerformance")]
        public string NetworkPerformance { get; set; }
    }
}