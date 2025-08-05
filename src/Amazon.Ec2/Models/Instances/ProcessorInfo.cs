using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class ProcessorInfo
{
    [XmlElement("sustainedClockSpeedInGhz")]
    public double? SustainedClockSpeedInGhz { get; init; }

    // i386 | x86_64 | arm64 | x86_64_mac | arm64_mac
    [XmlArray("supportedArchitectures")]
    [XmlArrayItem("item")]
    public string[]? SupportedArchitectures { get; init; }
}