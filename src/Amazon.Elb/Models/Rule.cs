#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Amazon.Elb
{
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

        [Range(1, 50_000)]
        [XmlElement]
        public int Priority { get; init; }

        [XmlElement]
        public string RuleArn { get; init; }
    }
}