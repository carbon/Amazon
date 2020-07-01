#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class ProcessorInfo
    {
        [XmlArray("supportedArchitectures")]
        [XmlArrayItem("item")]
        public string[] SupportedArchitectures { get; set; }
    }
}