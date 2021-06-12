#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class ModifyTargetGroupAttributesResponse : IElbResponse
    {
        [XmlElement]
        public ModifyTargetGroupAttributesResult ModifyTargetGroupAttributesResult { get; init; }
    }

    public sealed class ModifyTargetGroupAttributesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public TargetGroupAttribute[] Attributes { get; init; }
    }
}