#nullable disable

using System;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class VolumeAttachment
    {
        [XmlElement("volumeId")]
        public string VolumeId { get; set; }

        [XmlElement("instanceId")]
        public string InstanceId { get; set; }

        [XmlElement("device")]
        public string Device { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("attachTime")]
        public DateTime AttachTime { get; set; }
        
        [XmlElement("deleteOnTermination")]
        public bool DeleteOnTermination { get; set; }
    }
}
