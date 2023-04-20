#nullable disable

using System.Xml.Linq;

using static Amazon.CloudWatch.CloudWatchClient;

namespace Amazon.CloudWatch;

public sealed class GetMetricStatisticsResponse
{
    public string Label { get; set; }

    public List<DataPoint> Datapoints { get; } = new();

    public static GetMetricStatisticsResponse Deserialize(string xmlText)
    {
        var rootEl = XElement.Parse(xmlText); // ListMetricsResponse

        var resultEl = rootEl.Element(NS + "GetMetricStatisticsResult");

        var datapointsEl = resultEl.Element(NS + "Datapoints");

        var result = new GetMetricStatisticsResponse {
            Label = resultEl.Element(NS + "Label").Value
        };

        foreach (var point in datapointsEl.Elements())
        {
            result.Datapoints.Add(DataPoint.FromXElement(NS, point));
        }

        return result;
    }
}