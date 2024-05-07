using System.Xml.Serialization;

using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

public sealed class GetMetricStatisticsResponse
{
    public required GetMetricStatisticsResult GetMetricStatisticsResult { get; init; }

    public static GetMetricStatisticsResponse Deserialize(byte[] xmlText)
    {
        return CloudWatchSerializer<GetMetricStatisticsResponse>.DeserializeXml(xmlText);
    }
}

public sealed class GetMetricStatisticsResult
{
    [XmlElement]
    public required string Label { get; set; }

    [XmlArray]
    [XmlArrayItem("member")]
    public required List<DataPoint> Datapoints { get; init; }
}