#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class RunInstancesResponse : IEc2Response
{
    [XmlArray("instancesSet")]
    [XmlArrayItem("item")]
    public Instance[] Instances { get; init; }

    [XmlElement("ownerId")]
    public string OwnerId { get; init; }

    [XmlElement("requesterId")]
    public string RequesterId { get; init; }

    [XmlElement("reservationId")]
    public string ReservationId { get; init; }
}