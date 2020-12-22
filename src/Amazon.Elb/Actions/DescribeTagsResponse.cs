#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class DescribeTagsResponse : IElbResponse
    {
        [XmlElement]
        public DescribeTagsResult DescribeTagsResult { get; set; }
    }

    public sealed class DescribeTagsResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public TagDescription[] TagDescriptions { get; set; }
    }

    public sealed class TagDescription
    {
        [XmlElement]
        public string ResourceArn { get; set; }

        [XmlArray]
        [XmlArrayItem("member")]
        public Tag[] Tags { get; set; }
    }
}
