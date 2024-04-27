using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class TerminateInstancesResponse : IEc2Response
{
    [XmlArray("instancesSet")]
    [XmlArrayItem("item")]
    public required InstanceStateChange[] Instances { get; init; }
}