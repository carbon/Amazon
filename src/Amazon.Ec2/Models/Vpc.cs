using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class Vpc
    {
        [XmlElement("vpcId")]
        public string VpcId { get; set; }

        [XmlElement("cidrBlock")]
        public string CidrBlock { get; set; }

        // default | dedicated | host
        [XmlElement("instanceTenancy")]
        public string InstanceTenancy { get; set; }

        [XmlElement("isDefault")]
        public bool IsDefault { get; set; }

        // pending | available
        [XmlElement("state")]
        public string State { get; set; }
    }
}
