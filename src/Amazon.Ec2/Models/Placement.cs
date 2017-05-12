using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class Placement
    {
        [XmlElement("availabilityZone")]
        public string AvailabilityZone { get; set; }

        [XmlElement("groupName")]
        public string GroupName { get; set; }

        [XmlElement("tenancy")]
        public string Tenancy { get; set; }

        // hostId
        // groupName
    }
}