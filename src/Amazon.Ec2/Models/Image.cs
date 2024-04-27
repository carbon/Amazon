#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class Image
{
    // i386 | x86_64 | arm64 | x86_64_mac | arm64_mac
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

    // amazon | aws-marketplace
    [XmlElement("imageOwnerAlias")]
    public string ImageOwnerAlias { get; init; }

    /// <summary>
    /// The ID of the AWS account that owns the image.
    /// </summary>
    [XmlElement("imageOwnerId")]
    public string ImageOwnerId { get; init; }

    // pending | available | invalid | deregistered | transient | failed | error
    [XmlElement("imageState")]
    public string ImageState { get; init; }

    // machine | kernel | ramdisk
    [XmlElement("imageType")]
    public string ImageType { get; init; }

    [XmlElement("isPublic")]
    public bool IsPublic { get; init; }

#nullable enable
    [XmlElement("kernelId")]
    public string? KernelId { get; init; }
#nullable disable

    [XmlElement("name")]
    public string Name { get; init; }

#nullable enable
    // Windows | blank
    [XmlElement("platform")]
    public string? Platform { get; init; }
#nullable disable

    // ovm | xen
    [XmlElement("hypervisor")]
    public string Hypervisor { get; init; }

#nullable enable
    [XmlElement("ramDiskId")]
    public string? RamDiskId { get; init; }
#nullable disable

    // ebs | instance-store
    [XmlElement("rootDeviceType")]
    public string RootDeviceType { get; init; }

    [XmlElement("rootDeviceName")]
    public string RootDeviceName { get; init; }

    // hvm | paravirtual
    [XmlElement("virtualizationType")]
    public string VirtualizationType { get; init; }

    [XmlElement("creationDate")]
    public DateTime? CreationDate { get; init; }
}
