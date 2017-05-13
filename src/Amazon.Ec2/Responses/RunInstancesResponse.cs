using System.Collections.Generic;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class RunInstancesResponse : IEc2Response
    {
        [XmlArray("instancesSet")]
        [XmlArrayItem("item")]
        public List<Instance> Instances { get; } = new List<Instance>();

        [XmlElement("ownerId")]
        public string OwnerId { get; set; }
        
        [XmlElement("requesterId")]
        public string RequesterId { get; set; }

        [XmlElement("reservationId")]
        public string ReservationId { get; set; }
    }
}