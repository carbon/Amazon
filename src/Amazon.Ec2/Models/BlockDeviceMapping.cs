using System;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class BlockDeviceMapping
    {
        [XmlElement("deviceName")]
        public string DeviceName { get; set; }

        [XmlElement("ebs")]
        public Ebs Ebs { get; set; }
    }

    public class Ebs
    {
        [XmlElement("volumeId")]
        public string VolumeId { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("attachTime")]
        public DateTime AttachTime { get; set; }

        [XmlElement("deleteOnTermination")]
        public bool DeleteOnTermination { get; set; }
    }
}


/*
 <deviceName>/dev/xvda</deviceName>
            <ebs>
                <volumeId>vol-1234567890abcdef0</volumeId>
                <status>attached</status>
                <attachTime>2015-12-22T10:44:09.000Z</attachTime>
                <deleteOnTermination>true</deleteOnTermination>
            </ebs>
*/