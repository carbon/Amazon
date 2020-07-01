#nullable disable

using System;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class Volume
    {
        [XmlElement("volumeId")]
        public string VolumeId { get; set; }

        [XmlElement("iops")]
        public int? Iops { get; set; }

        [XmlElement("size")]
        public int Size { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("availabilityZone")]
        public string AvailabilityZone { get; set; }

        [XmlElement("volumeType")]
        public string VolumeType { get; set; } // standard | io1 | gp2 | sc1 | st1

        [XmlElement("encrypted")]
        public bool Encrypted { get; set; }

        [XmlElement("createTime", DataType = "dateTime")]
        public DateTime CreateTime { get; set; }

        [XmlArray("attachmentSet")]
        [XmlArrayItem("item")]
        public VolumeAttachment[] Attachments { get; set; }
    }
}
