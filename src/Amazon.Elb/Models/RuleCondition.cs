#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class RuleCondition
{
    // host-header, path-pattern
    [XmlElement]
    public string Field { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public string[] Values { get; init; }
}