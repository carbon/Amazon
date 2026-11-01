#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public class ModifyRuleResponse : IElbResponse
{
    [XmlElement]
    public ModifyRuleResult ModifyRuleResult { get; init; }
}

public sealed class ModifyRuleResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public Rule[] Rules { get; init; }
}
