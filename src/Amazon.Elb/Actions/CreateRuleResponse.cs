#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class CreateRuleResponse : IElbResponse
    {
        [XmlElement]
        public CreateRuleResult CreateRuleResult { get; init; }
    }

    public sealed class CreateRuleResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Rule[] Rules { get; init; }
    }
}