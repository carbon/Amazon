#nullable disable

using System.Collections.Generic;
using System.Xml.Linq;

using static Amazon.CloudWatch.CloudWatchClient;

namespace Amazon.CloudWatch;

public sealed class GetMetricStatatisticsResponse
{
    public string Label { get; set; }

    public List<DataPoint> Datapoints { get; } = new();

    public static GetMetricStatatisticsResponse Parse(string xmlText)
    {
        var result = new GetMetricStatatisticsResponse();

        var rootEl = XElement.Parse(xmlText); // ListMetricsResponse

        var resultEl = rootEl.Element(NS + "GetMetricStatisticsResult");

        var datapointsEl = resultEl.Element(NS + "Datapoints");

        result.Label = resultEl.Element(NS + "Label").Value;

        foreach (var datapoint in datapointsEl.Elements())
        {
            result.Datapoints.Add(DataPoint.FromXml(NS, datapoint));
        }

        return result;
    }
}

/*
<GetMetricStatisticsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
  <GetMetricStatisticsResult>
    <Datapoints>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T12:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T17:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T13:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T11:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T16:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.894345671815797</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T21:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.894342592322714</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T19:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T10:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T15:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T20:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T18:01:00Z</Timestamp>
      </member>
      <member>
        <Average>3.895104895104895</Average>
        <Unit>Count</Unit>
        <Timestamp>2016-11-02T14:01:00Z</Timestamp>
      </member>
    </Datapoints>
    <Label>HealthyHostCount</Label>
  </GetMetricStatisticsResult>
  <ResponseMetadata>
    <RequestId>ef302b1f-a147-11e6-acab-872d798a60c5</RequestId>
  </ResponseMetadata>
</GetMetricStatisticsResponse>


*/
