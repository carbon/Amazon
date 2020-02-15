#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class ModifyTargetGroupAttributesResponse : IElbResponse
    {
        [XmlElement]
        public ModifyTargetGroupAttributesResult ModifyTargetGroupAttributesResult { get; set; }
    }

    public class ModifyTargetGroupAttributesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public TargetGroupAttribute[] Attributes { get; set; }
    }
}