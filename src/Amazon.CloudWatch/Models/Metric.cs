using System.Xml.Serialization;

namespace Amazon.CloudWatch;

public sealed class Metric
{
    [XmlElement]
    public required string Namespace { get; init; }

    [XmlElement]
    public required string MetricName { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public required List<Dimension> Dimensions { get; init; }
}

/*
<member>
    <Namespace>AWS/ELB</Namespace>
    <MetricName>BackendConnectionErrors</MetricName>
    <Dimensions>
        <member>
          <Name>Namespace</Name>
          <Value>AWS</Value>
        </member>
    </Dimensions>
</member>
*/