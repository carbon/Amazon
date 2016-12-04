using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class Volume
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
        public List<VolumeAttachment> Attachments { get; set; }
    }

    public class VolumeAttachment
    {
        [XmlElement("attachTime", DataType = "dateTime")]
        
        public DateTime AttachTime { get; set; }

        [XmlElement("volumeId")]
        public string VolumeId { get; set; }

        [XmlElement("instanceId")]
        public string InstanceId { get; set; }

        [XmlElement("device")]
        public string Device { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("deleteOnTermination")]
        public bool DeleteOnTermination { get; set; }
    }
}
