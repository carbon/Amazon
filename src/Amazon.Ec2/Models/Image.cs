using System.Xml.Linq;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class Image
    {
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

        [XmlElement("imageState")]
        public string ImageState { get; set; }

        [XmlElement("imageType")]
        public string ImageType { get; set; }

        [XmlElement("isPublic")]
        public bool IsPublic { get; set; }

        [XmlElement("kernelId")]
        public string KernelId { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("architecture")]
        public string Architecture { get; set; }

        [XmlElement("platform")]
        public string Platform { get; set; }

        [XmlElement("hypervisor")]
        public string Hypervisor { get; set; }

        [XmlElement("ramDiskId")]
        public string RamDiskId { get; set; }

        [XmlElement("rootDeviceType")]
        public string RootDeviceType { get; set; }

        [XmlElement("virtualizationType")]
        public string VirtualizationType { get; set; }

        private static readonly XmlSerializer serializer = new XmlSerializer(
            typeof(Image),
            new XmlRootAttribute {
                ElementName = "item",
                Namespace = Ec2Client.Namespace
            }
        );

        public static Image Deserialize(XElement element)
        {
            return (Image)serializer.Deserialize(element.CreateReader());
        }
    }
}
