#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class EbsBlockDevice
    {
        public EbsBlockDevice() { }

        public EbsBlockDevice(
            string volumeId = null,
            bool? deleteOnTermination = null,
            bool? encrypted = null,
            int? iops = null,
            string snapshotId = null,
            int? volumeSize = null,
            string volumeType = null)
        {
            VolumeId            = volumeId;
            DeleteOnTermination = deleteOnTermination;
            Encrypted           = encrypted;
            Iops                = iops;
            SnapshotId          = snapshotId;
            VolumeSize          = volumeSize;
            VolumeType          = volumeType;
        }

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

        // [Range(100, 20000)]
        [XmlElement("iops")]
        public int? Iops { get; set; }

        [XmlElement("snapshotId")]
        public string SnapshotId { get; set; }

        // The size of the volume, in GiB.
        // [Range(1, 16384)]
        [XmlElement("volumeSize")]
        public int? VolumeSize { get; set; }

        // gp2, io1, st1, sc1, or standard.
        [XmlElement("volumeType")]
        public string VolumeType { get; set; }
    }
}


/*
<ebs>
  <volumeId>vol-1234567890abcdef0</volumeId>
  <status>attached</status>
  <attachTime>2015-12-22T10:44:09.000Z</attachTime>
  <deleteOnTermination>true</deleteOnTermination>
</ebs>
*/