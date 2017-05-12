using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class DescribeTargetGroupsResponse : IElbResponse
    {
        [XmlElement]
        public DescribeTargetGroupsResult DescribeTargetGroupsResult { get; set; }
    }

    public class DescribeTargetGroupsResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public TargetGroup[] TargetGroups { get; set; }
    }
}