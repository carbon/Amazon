#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs;

public readonly struct SqsSystemMessageAttribute
{
    [XmlElement("Name")]
    public string Name { get; init; }

    [XmlElement("Value")]
    public string Value { get; init; }
}
