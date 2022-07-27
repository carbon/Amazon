#nullable disable

using System.Xml.Linq;

using static Amazon.CloudWatch.CloudWatchClient;

namespace Amazon.CloudWatch;

public static class ListMetricsResponse
{
    public static List<Metric> Parse(string xmlText)
    {
        var metrics = new List<Metric>();

        var rooteEl = XElement.Parse(xmlText); // ListMetricsResponse

        var listMetricsResultEl = rooteEl.Element(NS + "ListMetricsResult");
        var metricsEl = listMetricsResultEl.Element(NS + "Metrics");

        foreach (var metricEl in metricsEl.Elements()) // member...
        {
            var metric = Metric.FromXml(NS, metricEl);

            metrics.Add(metric);
        }

        return metrics;
    }
}

/*
<ListMetricsResponse xmlns=""http://monitoring.amazonaws.com/doc/2010-08-01/"">
  <ListMetricsResult>
    <Metrics>
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
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>HTTPCode_ELB_5XX</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>HealthyHostCount</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>HTTPCode_Backend_2XX</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>UnHealthyHostCount</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>SurgeQueueLength</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>HTTPCode_Backend_5XX</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>HTTPCode_Backend_4XX</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>HTTPCode_Backend_3XX</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>Latency</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
      <member>
        <Namespace>AWS/ELB</Namespace>
        <MetricName>RequestCount</MetricName>
        <Dimensions>
          <member>
            <Name>Namespace</Name>
            <Value>AWS</Value>
          </member>
        </Dimensions>
      </member>
    </Metrics>
  </ListMetricsResult>
  <ResponseMetadata>
    <RequestId>c919ffb2-a13d-11e6-994f-1d1bb7ed25dd</RequestId>
  </ResponseMetadata>
</ListMetricsResponse>
*/
