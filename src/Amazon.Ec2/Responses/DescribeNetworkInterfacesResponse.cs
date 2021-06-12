#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class DescribeNetworkInterfacesResponse : IEc2Response
    {
        [XmlArray("networkInterfaceSet")]
        [XmlArrayItem("item")]
        public NetworkInterface[] NetworkInterfaces { get; init; }
    }
}