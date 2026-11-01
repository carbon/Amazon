#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class Rule
{
    [XmlArray]
    [XmlArrayItem("member")]
    public Action[] Actions { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public RuleCondition[] Conditions { get; init; }

    [XmlElement]
    public bool IsDefault { get; init; }

    // 1-50000, or "default" for the default rule
    [XmlElement]
    public string Priority { get; init; }

    [XmlElement]
    public string RuleArn { get; init; }
}