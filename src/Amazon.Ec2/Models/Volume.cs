#nullable disable

using System;
using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class Volume
{
    [XmlElement("volumeId")]
    public string VolumeId { get; init; }

    [XmlElement("iops")]
    public int? Iops { get; init; }

    [XmlElement("size")]
    public int Size { get; init; }

    [XmlElement("status")]
    public string Status { get; init; }

    [XmlElement("availabilityZone")]
    public string AvailabilityZone { get; init; }

    [XmlElement("volumeType")]
    public string VolumeType { get; init; } // standard | io1 | gp2 | sc1 | st1

    [XmlElement("encrypted")]
    public bool Encrypted { get; init; }

    [XmlElement("createTime", DataType = "dateTime")]
    public DateTime CreateTime { get; init; }

    [XmlArray("attachmentSet")]
    [XmlArrayItem("item")]
    public VolumeAttachment[] Attachments { get; init; }
}
