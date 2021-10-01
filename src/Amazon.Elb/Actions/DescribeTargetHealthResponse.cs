#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class DescribeTargetHealthResponse : IElbResponse
{
    [XmlElement]
    public DescribeTargetHealthResult DescribeTargetHealthResult { get; init; }
}

public sealed class DescribeTargetHealthResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public TargetHealthDescription[] TargetHealthDescriptions { get; init; }
}

public sealed class TargetHealthDescription
{
    [XmlElement]
    public string HealthCheckPort { get; init; }

    [XmlElement]
    public TargetHealth TargetHealth { get; init; }

    [XmlElement]
    public TargetDescription Target { get; init; }
}