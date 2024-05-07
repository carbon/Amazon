using System.Xml.Serialization;

namespace Amazon.CloudWatch;

public readonly struct Dimension
{
    [XmlElement]
    public required string Name { get; init; }

    [XmlElement]
    public required string Value { get; init; }
}
