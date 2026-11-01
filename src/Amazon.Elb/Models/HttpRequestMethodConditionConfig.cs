using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class HttpRequestMethodConditionConfig
{
    [XmlArray]
    [XmlArrayItem("member")]
    public string[]? Values { get; init; }
}
