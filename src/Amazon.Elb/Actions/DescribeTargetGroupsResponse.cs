using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class DescribeTargetGroupsResponse : IElbResponse
{
    [XmlElement]
    public required DescribeTargetGroupsResult DescribeTargetGroupsResult { get; init; }
}

public sealed class DescribeTargetGroupsResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public required TargetGroup[] TargetGroups { get; init; }
}