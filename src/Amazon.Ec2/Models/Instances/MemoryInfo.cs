using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class MemoryInfo
    {
        [XmlElement("sizeInMiB")]
        public int SizeInMiB { get; set; }
    }
}
