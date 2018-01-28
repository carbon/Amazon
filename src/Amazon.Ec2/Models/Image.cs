using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class Image
    {
        // i386 | x86_64
        [XmlElement("architecture")]
        public string Architecture { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("enaSupport")]
        public bool EnaSupport { get; set; }

        [XmlElement("imageId")]
        public string ImageId { get; set; }

        [XmlElement("imageLocation")]
        public string ImageLocation { get; set; }

        [XmlElement("imageOwnerAlias")]
        public string ImageOwnerAlias { get; set; }

        [XmlElement("imageOwnerId")]
        public long ImageOwnerId { get; set; }

        // pending | available | invalid | deregistered | transient | failed | error
        [XmlElement("imageState")]
        public string ImageState { get; set; }

        // machine | kernel | ramdisk
        [XmlElement("imageType")]
        public string ImageType { get; set; }

        [XmlElement("isPublic")]
        public bool IsPublic { get; set; }

        [XmlElement("kernelId")]
        public string KernelId { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        // Windows | blank
        [XmlElement("platform")]
        public string Platform { get; set; }

        // ovm | xen
        [XmlElement("hypervisor")]
        public string Hypervisor { get; set; }

        [XmlElement("ramDiskId")]
        public string RamDiskId { get; set; }

        // ebs | instance-store
        [XmlElement("rootDeviceType")]
        public string RootDeviceType { get; set; }

        // hvm | paravirtual
        [XmlElement("virtualizationType")]
        public string VirtualizationType { get; set; }
    }
}
