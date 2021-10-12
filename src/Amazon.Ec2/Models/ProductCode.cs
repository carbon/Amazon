#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class ProductCode
{
    [XmlElement("productCode")]
    public string Value { get; init; }

    [XmlElement("type")]
    public string Type { get; init; }
}
