using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class ForwardActionConfig
{
    public TargetGroupStickinessConfig? TargetGroupStickinessConfig { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public TargetGroupTuple[]? TargetGroups { get; init; }
}
