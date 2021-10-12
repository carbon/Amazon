#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class Image
{
    // i386 | x86_64
    [XmlElement("architecture")]
    public string Architecture { get; init; }

    [XmlElement("description")]
    public string Description { get; init; }

    [XmlElement("enaSupport")]
    public bool EnaSupport { get; init; }

    [XmlElement("imageId")]
    public string ImageId { get; init; }

    [XmlElement("imageLocation")]
    public string ImageLocation { get; init; }

    [XmlElement("imageOwnerAlias")]
    public string ImageOwnerAlias { get; init; }

    [XmlElement("imageOwnerId")]
    public long ImageOwnerId { get; init; }

    // pending | available | invalid | deregistered | transient | failed | error
    [XmlElement("imageState")]
    public string ImageState { get; init; }

    // machine | kernel | ramdisk
    [XmlElement("imageType")]
    public string ImageType { get; init; }

    [XmlElement("isPublic")]
    public bool IsPublic { get; init; }

    [XmlElement("kernelId")]
    public string KernelId { get; init; }

    [XmlElement("name")]
    public string Name { get; init; }

    // Windows | blank
    [XmlElement("platform")]
    public string Platform { get; init; }

    // ovm | xen
    [XmlElement("hypervisor")]
    public string Hypervisor { get; init; }

    [XmlElement("ramDiskId")]
    public string RamDiskId { get; init; }

    // ebs | instance-store
    [XmlElement("rootDeviceType")]
    public string RootDeviceType { get; init; }

    [XmlElement("rootDeviceName")]
    public string RootDeviceName { get; init; }

    // hvm | paravirtual
    [XmlElement("virtualizationType")]
    public string VirtualizationType { get; init; }
}
