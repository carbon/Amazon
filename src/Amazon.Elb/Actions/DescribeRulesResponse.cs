#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class DescribeRulesResponse : IElbResponse
    {
        [XmlElement]
        public DescribeRulesResult DescribeRulesResult { get; init; }
    }

    public sealed class DescribeRulesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Rule[] Rules { get; init; }
    }
}