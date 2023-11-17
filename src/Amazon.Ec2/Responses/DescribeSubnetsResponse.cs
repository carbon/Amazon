#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class DescribeSubnetsResponse : IEc2Response
{
    [XmlArray("subnetSet")]
    [XmlArrayItem("item")]
    public Subnet[] Subnets { get; init; }
}