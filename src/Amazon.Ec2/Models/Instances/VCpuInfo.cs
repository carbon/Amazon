#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class VCpuInfo
    {
        [XmlElement("defaultCores")]
        public int DefaultCores { get; init; }

        [XmlElement("defaultThreadsPerCore")]
        public int DefaultThreadsPerCore { get; init; }

        [XmlElement("defaultVCpus")]
        public int DefaultVCpus { get; init; }

        [XmlArray("validCores")]
        [XmlArrayItem("item")]
        public int[] ValidCores { get; init; }

        [XmlArray("validThreadsPerCore")]
        [XmlArrayItem("item")]
        public int[] ValidThreadsPerCore { get; init; }
    }
}

