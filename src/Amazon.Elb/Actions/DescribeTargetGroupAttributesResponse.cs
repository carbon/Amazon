using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class DescribeTargetGroupAttributesResponse : IElbResponse
{
    [XmlElement]
    public required DescribeTargetGroupAttributesResult DescribeTargetGroupAttributesResult { get; init; }
}

public sealed class DescribeTargetGroupAttributesResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public required TargetGroupAttribute[] Attributes { get; init; }
}