#nullable disable

using System.Xml.Serialization;

using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

public sealed class GetMetricDataResponse
{
    public GetMetricDataResult GetMetricDataResult { get; init; }

    public static GetMetricDataResponse Deserialize(byte[] xmlText)
    {
        return CloudWatchSerializer<GetMetricDataResponse>.DeserializeXml(xmlText);
    }
}

public sealed class GetMetricDataResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public List<MessageData> Messages { get; set; }

    [XmlArray]
    [XmlArrayItem("member")]
    public List<MetricDataResult> MetricDataResults { get; set; }

    /// <summary>
    /// The token to pass as <see cref="GetMetricDataRequest.NextToken"/> for the next page. Null on the last page.
    /// </summary>
    public string NextToken { get; set; }
}
