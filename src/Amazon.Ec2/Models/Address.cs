#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class Address
    {
        [XmlElement("allocationId")]
        public string AllocationId { get; init; }

        [XmlElement("allocationId")]
        public string AssociationId { get; init; }

        [XmlElement("domain")]
        public string Domain { get; init; }

        [XmlElement("instanceId")]
        public string InstanceId { get; init; }

        [XmlElement("networkInterfaceId")]
        public string NetworkInterfaceId { get; init; }

        [XmlElement("networkInterfaceOwnerId")]
        public string NetworkInterfaceOwnerId { get; init; }

        [XmlElement("privateIpAddress")]
        public string PrivateIpAddress { get; init; }

        [XmlElement("publicIp")]
        public string PublicIp { get; init; }
    }
}
