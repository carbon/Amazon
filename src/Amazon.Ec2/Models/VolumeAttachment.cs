#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class VolumeAttachment
{
    [XmlElement("volumeId")]
    public string VolumeId { get; init; }

    [XmlElement("instanceId")]
    public string InstanceId { get; init; }

    [XmlElement("device")]
    public string Device { get; init; }

    [XmlElement("status")]
    public string Status { get; init; }

    [XmlElement("attachTime")]
    public DateTime AttachTime { get; init; }

    [XmlElement("deleteOnTermination")]
    public bool DeleteOnTermination { get; init; }
}
