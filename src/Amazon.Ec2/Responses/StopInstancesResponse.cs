using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class StopInstancesResponse : IEc2Response
    {
        [XmlArray("instancesSet")]
        [XmlArrayItem("item")]
        public InstanceStateChange[] Instances { get; set; }
    }
}