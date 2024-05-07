#nullable disable

using System.Xml.Serialization;

using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

public class ListMetricsResponse
{
    [XmlElement]
    public ListMetricsResult ListMetricsResult { get; init; }

    public static List<Metric> Parse(byte[] xmlText)
    {
        var response = CloudWatchSerializer<ListMetricsResponse>.DeserializeXml(xmlText);

        return response.ListMetricsResult.Metrics;
    }
}

public sealed class ListMetricsResult
{
    [XmlArray]
    [XmlArrayItem("member")]
    public List<Metric> Metrics { get; set; }
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
