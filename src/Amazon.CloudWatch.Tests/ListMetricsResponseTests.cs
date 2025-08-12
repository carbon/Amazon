namespace Amazon.CloudWatch.Tests;

public class ListMetricsResponseTests
{
    [Fact]
    public void ParseResponse()
    {
        var result = ListMetricsResponse.Parse(
            """
            <ListMetricsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
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
            """u8.ToArray());

        Assert.Equal(11, result.Count);

        Assert.Equal("AWS/ELB"                 , result[0].Namespace);
        Assert.Equal("BackendConnectionErrors" , result[0].MetricName);

        Assert.Single(result[0].Dimensions);

        Assert.Equal("Namespace"               , result[0].Dimensions[0].Name);
        Assert.Equal("AWS"                     , result[0].Dimensions[0].Value);
    }
}