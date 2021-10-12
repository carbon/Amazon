#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class StartInstancesResponse : IEc2Response
{
    [XmlArray("instancesSet")]
    [XmlArrayItem("item")]
    public InstanceStateChange[] Instances { get; set; }
}