#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class TargetGroupAttribute
{
    [XmlElement]
    public string Key { get; init; }

    [XmlElement]
    public string Value { get; init; }
}