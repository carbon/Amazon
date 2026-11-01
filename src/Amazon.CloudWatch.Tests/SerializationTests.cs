using System.Text;

using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch.Tests;

public class SerializationTests
{
    [Fact]
    public void ParsesEveryDataPointField()
    {
        var response = GetMetricStatisticsResponse.Deserialize(
            """
            <GetMetricStatisticsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <GetMetricStatisticsResult>
                <Datapoints>
                  <member>
                    <Average>1.5</Average>
                    <Maximum>2.0E7</Maximum>
                    <Minimum>-3.5E-4</Minimum>
                    <SampleCount>4.0</SampleCount>
                    <Sum>6</Sum>
                    <Unit>Bytes/Second</Unit>
                    <Timestamp>2016-11-02T12:01:30Z</Timestamp>
                  </member>
                </Datapoints>
                <Label>Throughput</Label>
              </GetMetricStatisticsResult>
            </GetMetricStatisticsResponse>
            """u8.ToArray());

        var point = Assert.Single(response.GetMetricStatisticsResult.Datapoints);

        Assert.Equal(1.5d, point.Average);
        Assert.Equal(2.0E7d, point.Maximum);
        Assert.Equal(-3.5E-4d, point.Minimum);
        Assert.Equal(4d, point.SampleCount);
        Assert.Equal(6d, point.Sum);
        Assert.Equal("Bytes/Second", point.Unit);
        Assert.Equal(new DateTime(2016, 11, 02, 12, 01, 30), point.Timestamp);
        Assert.Equal(DateTimeKind.Utc, point.Timestamp.Kind);
    }

    [Fact]
    public void LeavesUnrequestedStatisticsNull()
    {
        var response = GetMetricStatisticsResponse.Deserialize(
            """
            <GetMetricStatisticsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <GetMetricStatisticsResult>
                <Datapoints>
                  <member>
                    <Sum>6</Sum>
                    <Timestamp>2016-11-02T12:01:30Z</Timestamp>
                  </member>
                </Datapoints>
                <Label>Throughput</Label>
              </GetMetricStatisticsResult>
            </GetMetricStatisticsResponse>
            """u8.ToArray());

        var point = Assert.Single(response.GetMetricStatisticsResult.Datapoints);

        Assert.Null(point.Average);
        Assert.Null(point.Maximum);
        Assert.Null(point.Minimum);
        Assert.Null(point.SampleCount);
    }

    [Fact]
    public void ParsesEmptyDatapoints()
    {
        var response = GetMetricStatisticsResponse.Deserialize(
            """
            <GetMetricStatisticsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <GetMetricStatisticsResult>
                <Datapoints />
                <Label>Throughput</Label>
              </GetMetricStatisticsResult>
            </GetMetricStatisticsResponse>
            """u8.ToArray());

        Assert.Empty(response.GetMetricStatisticsResult.Datapoints);
    }

    [Fact]
    public void RejectsUnexpectedRootElement()
    {
        Assert.Throws<InvalidOperationException>(() => ListMetricsResponse.Parse(
            """<ErrorResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/" />"""u8.ToArray()));
    }

    [Fact]
    public void RejectsMalformedXml()
    {
        Assert.Throws<InvalidOperationException>(() => ListMetricsResponse.Parse("<ListMetricsResponse"u8.ToArray()));
    }

    [Fact]
    public void SerializedMetricRoundTrips()
    {
        var metric = new Metric {
            Namespace = "AWS/ELB",
            MetricName = "Latency",
            Dimensions = [new Dimension { Name = "LoadBalancerName", Value = "web" }]
        };

        var result = CloudWatchSerializer<Metric>.DeserializeXml(CloudWatchSerializer<Metric>.SerializeToUtf8Bytes(metric));

        Assert.Equal("AWS/ELB", result.Namespace);
        Assert.Equal("Latency", result.MetricName);
        Assert.Equal("web", Assert.Single(result.Dimensions).Value);
    }

    [Fact]
    public void SerializesWithoutBomAndInServiceNamespace()
    {
        var bytes = CloudWatchSerializer<Metric>.SerializeToUtf8Bytes(new Metric {
            Namespace = "N",
            MetricName = "M",
            Dimensions = []
        });

        var xml = Encoding.UTF8.GetString(bytes);

        Assert.StartsWith("<?xml", xml);
        Assert.Contains("""<Metric xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">""", xml);
    }
}
