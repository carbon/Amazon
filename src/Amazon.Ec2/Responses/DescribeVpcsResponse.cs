using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class DescribeVpcsResponse : IEc2Response
{
    [XmlArray("vpcSet")]
    [XmlArrayItem("item")]
    public required Vpc[] Vpcs { get; init; }
}