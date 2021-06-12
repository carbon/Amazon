#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class StopInstancesResponse : IEc2Response
    {
        [XmlArray("instancesSet")]
        [XmlArrayItem("item")]
        public InstanceStateChange[] Instances { get; init; }
    }
}