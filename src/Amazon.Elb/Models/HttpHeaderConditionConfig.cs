using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class HttpHeaderConditionConfig
{
    public string? HttpHeaderName { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public string[]? RegexValues { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public string[]? Values { get; init; }
}
