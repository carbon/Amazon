using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class HostHeaderConditionConfig
{
    [XmlArray]
    [XmlArrayItem("member")]
    public string[]? RegexValues { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public string[]? Values { get; init; }
}
