#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class DescribeTargetHealthResponse : IElbResponse
    {
        [XmlElement]
        public DescribeTargetHealthResult DescribeTargetHealthResult { get; set; }
    }

    public sealed class DescribeTargetHealthResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public TargetHealthDescription[] TargetHealthDescriptions { get; set; }
    }

    public sealed class TargetHealthDescription
    {
        [XmlElement]
        public string HealthCheckPort { get; set; }

        [XmlElement]
        public TargetHealth TargetHealth  { get; set; }

        [XmlElement]
        public TargetDescription Target { get; set; }
    }
}