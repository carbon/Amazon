#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class ModifyTargetGroupResponse : IElbResponse
    {
        [XmlElement]
        public ModifyTargetGroupResult ModifyTargetGroupResult { get; init; }
    }

    public sealed class ModifyTargetGroupResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public TargetGroup[] TargetGroups { get; init; }
    }

}