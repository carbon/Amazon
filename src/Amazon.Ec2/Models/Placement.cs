#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class Placement
    {
        public Placement() { }

        public Placement(
            string availabilityZone = null, 
            string groupName = null,
            string hostId = null, 
            string tenancy = null)
        {
            AvailabilityZone = availabilityZone;
            GroupName        = groupName;
            HostId           = hostId;
            Tenancy          = tenancy;
        }

        [XmlElement("availabilityZone")]
        public string AvailabilityZone { get; set; }

        [XmlElement("groupName")]
        public string GroupName { get; set; }

        [XmlElement("hostId")]
        public string HostId { get; set; }

        [XmlElement("tenancy")]
        public string Tenancy { get; set; }
    }
}