#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class RunInstancesResponse : IEc2Response
    {
        [XmlArray("instancesSet")]
        [XmlArrayItem("item")]
        public Instance[] Instances { get; set; }

        [XmlElement("ownerId")]
        public string OwnerId { get; set; }
        
        [XmlElement("requesterId")]
        public string RequesterId { get; set; }

        [XmlElement("reservationId")]
        public string ReservationId { get; set; }
    }
}