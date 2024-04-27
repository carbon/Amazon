using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class DescribeNetworkInterfacesResponse : IEc2Response
{
    [XmlArray("networkInterfaceSet")]
    [XmlArrayItem("item")]
    public required NetworkInterface[] NetworkInterfaces { get; init; }
}
