#nullable disable

using System.Xml.Serialization;

namespace Amazon.CloudWatch;

public sealed class MetricDataResult
{
    public string Id { get; set; }

    public string Label { get; set; }

    [XmlArray]
    [XmlArrayItem("member")]
    public List<MessageData> Messages { get; set; }

    /// <summary>
    /// Complete, PartialData, InternalError or Forbidden.
    /// </summary>
    public string StatusCode { get; set; }

    /// <summary>
    /// The timestamps of the data points. Timestamps[x] is the time of Values[x].
    /// </summary>
    [XmlArray]
    [XmlArrayItem("member")]
    public List<DateTime> Timestamps { get; set; }

    [XmlArray]
    [XmlArrayItem("member")]
    public List<double> Values { get; set; }
}
