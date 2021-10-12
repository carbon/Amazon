#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class EbsBlockDevice
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
    public string VolumeId { get; init; }

    [XmlElement("status")]
    public string Status { get; init; }

    [XmlElement("attachTime")]
    public DateTime? AttachTime { get; init; }

    [XmlElement("deleteOnTermination")]
    public bool? DeleteOnTermination { get; init; }

    [XmlElement("encrypted")]
    public bool? Encrypted { get; init; }

    // [Range(100, 20000)]
    [XmlElement("iops")]
    public int? Iops { get; init; }

    [XmlElement("snapshotId")]
    public string SnapshotId { get; init; }

    // The size of the volume, in GiB.
    // [Range(1, 16384)]
    [XmlElement("volumeSize")]
    public int? VolumeSize { get; init; }

    // gp2, io1, st1, sc1, or standard.
    [XmlElement("volumeType")]
    public string VolumeType { get; init; }
}

/*
<ebs>
  <volumeId>vol-1234567890abcdef0</volumeId>
  <status>attached</status>
  <attachTime>2015-12-22T10:44:09.000Z</attachTime>
  <deleteOnTermination>true</deleteOnTermination>
</ebs>
*/