using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class DescribeSecurityGroupsResponse : IEc2Response
{
    [XmlArray("securityGroupInfo")]
    [XmlArrayItem("item")]
    public required SecurityGroup[] SecurityGroups { get; init; }
}