#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class NetworkInfo
    {
        [XmlElement("enaSupport")]
        public string EnaSupport { get; init; }

        [XmlElement("ipv4AddressesPerInterface")]
        public int Ipv4AddressesPerInterface { get; init; }

        [XmlElement("ipv6AddressesPerInterface")]
        public int Ipv6AddressesPerInterface { get; init; }

        [XmlElement("ipv6Supported")]
        public bool Ipv6Supported { get; init; }

        [XmlElement("maximumNetworkInterfaces")]
        public int MaximumNetworkInterfaces { get; init; }

        [XmlElement("networkPerformance")]
        public string NetworkPerformance { get; init; }
    }
}