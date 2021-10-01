#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class DescribeTagsResponse : IElbResponse
{
    [XmlElement]
    public DescribeTagsResult DescribeTagsResult { get; init; }
}

public sealed class DescribeTagsResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public TagDescription[] TagDescriptions { get; init; }
}

public sealed class TagDescription
{
    [XmlElement]
    public string ResourceArn { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public Tag[] Tags { get; init; }
}