#nullable disable

using System.Xml.Serialization;

using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

public sealed class DescribeAlarmsResponse
{
    public DescribeAlarmsResult DescribeAlarmsResult { get; init; }

    public static DescribeAlarmsResponse Deserialize(byte[] xmlText)
    {
        return CloudWatchSerializer<DescribeAlarmsResponse>.DeserializeXml(xmlText);
    }
}

public sealed class DescribeAlarmsResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public List<MetricAlarm> MetricAlarms { get; set; }

    /// <summary>
    /// The token to pass as <see cref="DescribeAlarmsRequest.NextToken"/> for the next page. Null on the last page.
    /// </summary>
    public string NextToken { get; set; }
}
