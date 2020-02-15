#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class ModifyTargetGroupResponse : IElbResponse
    {
        [XmlElement]
        public ModifyTargetGroupResult ModifyTargetGroupResult { get; set; }
    }

    public class ModifyTargetGroupResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public TargetGroup[] TargetGroups { get; set; }
    }

}