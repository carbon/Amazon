using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class BlockDeviceMapping
    {
        [XmlElement("deviceName")]
        public string DeviceName { get; set; }

        [XmlElement("ebs")]
        public EbsBlockDevice Ebs { get; set; }

        [XmlElement("virtualName")]
        public string VirtualName { get; set; }
    }

    public class EbsBlockDevice
    {
        [XmlElement("volumeId")]
        public string VolumeId { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("attachTime")]
        public DateTime? AttachTime { get; set; }

        [XmlElement("deleteOnTermination")]
        public bool? DeleteOnTermination { get; set; }

        [XmlElement("encrypted")]
        public bool? Encrypted { get; set; }

        [Range(100, 20000)]
        public int? Iops { get; set; }

        public string SnapshotId { get; set; }

        // The size of the volume, in GiB.
        [Range(1, 16384)]
        public int? VolumeSize { get; set; }

        // gp2, io1, st1, sc1, or standard.
        public string VolumeType { get; set; }
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