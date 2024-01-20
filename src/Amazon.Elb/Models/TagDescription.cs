#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class TagDescription
{
    [XmlElement]
    public string ResourceArn { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public Tag[] Tags { get; init; }
}