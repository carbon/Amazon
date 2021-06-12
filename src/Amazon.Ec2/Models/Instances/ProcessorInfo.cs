using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class ProcessorInfo
    {
        [XmlElement("sustainedClockSpeedInGhz")]
        public double? SustainedClockSpeedInGhz { get; init; }

        [XmlArray("supportedArchitectures")]
        [XmlArrayItem("item")]
        public string[]? SupportedArchitectures { get; init; }
    }
}