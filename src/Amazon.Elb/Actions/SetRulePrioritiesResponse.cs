using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class SetRulePrioritiesResponse : IElbResponse
    {
        [XmlElement]
        public SetRulePrioritiesResult SetRulePrioritiesResult { get; set; }
    }

    public class SetRulePrioritiesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Rule[] Rules { get; set; }
    }
}