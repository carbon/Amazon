using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class AvailabilityZone
    {
        [XmlElement]
        public string SubnetId { get; set; }

        [XmlElement]
        public string ZoneName { get; set; }
    }
}