#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class DescribeTagsResponse : IElbResponse
{
    [XmlElement]
    public required DescribeTagsResult DescribeTagsResult { get; init; }
}

public sealed class DescribeTagsResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public required TagDescription[] TagDescriptions { get; init; }
}