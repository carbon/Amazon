#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class DescribeTargetGroupsResponse : IElbResponse
{
    [XmlElement]
    public DescribeTargetGroupsResult DescribeTargetGroupsResult { get; init; }
}

public sealed class DescribeTargetGroupsResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public TargetGroup[] TargetGroups { get; init; }
}