#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class SetRulePrioritiesResponse : IElbResponse
{
    [XmlElement]
    public SetRulePrioritiesResult SetRulePrioritiesResult { get; init; }
}

public sealed class SetRulePrioritiesResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public Rule[] Rules { get; init; }
}