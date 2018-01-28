using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class StartInstancesResponse : IEc2Response
    {
        [XmlArray("instancesSet")]
        [XmlArrayItem("item")]
        public InstanceStateChange[] Instances { get; set; }
    }
}