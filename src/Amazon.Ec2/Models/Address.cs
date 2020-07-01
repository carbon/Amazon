#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class Address
    {
        [XmlElement("allocationId")]
        public string AllocationId { get; set; }

        [XmlElement("allocationId")]
        public string AssociationId { get; set; }

        [XmlElement("domain")]
        public string Domain { get; set; }

        [XmlElement("instanceId")]
        public string InstanceId { get; set; }

        [XmlElement("networkInterfaceId")]
        public string NetworkInterfaceId { get; set; }

        [XmlElement("networkInterfaceOwnerId")]
        public string NetworkInterfaceOwnerId { get; set; }

        [XmlElement("privateIpAddress")]
        public string PrivateIpAddress { get; set; }

        [XmlElement("publicIp")]
        public string PublicIp { get; set; }
    }
}
