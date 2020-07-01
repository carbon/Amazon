#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class VCpuInfo
    {
        [XmlElement("defaultCores")]
        public int DefaultCores { get; set; }

        [XmlElement("defaultThreadsPerCore")]
        public int DefaultThreadsPerCore { get; set; }

        [XmlElement("defaultVCpus")]
        public int DefaultVCpus { get; set; }

        [XmlArray("validCores")]
        [XmlArrayItem("item")]
        public int[] ValidCores { get; set; }

        [XmlArray("validThreadsPerCore")]
        [XmlArrayItem("item")]
        public int[] ValidThreadsPerCore { get; set; }
    }
}

