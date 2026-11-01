using System.Net;

namespace Amazon.CloudWatch.Tests;

public class ListMetricsTests
{
    private const string OneMetric =
        """
        <ListMetricsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
          <ListMetricsResult>
            <Metrics>
              <member>
                <Namespace>AWS/ELB</Namespace>
                <MetricName>Latency</MetricName>
                <Dimensions>
                  <member>
                    <Name>LoadBalancerName</Name>
                    <Value>web</Value>
                  </member>
                  <member>
                    <Name>AvailabilityZone</Name>
                    <Value>us-east-1a</Value>
                  </member>
                </Dimensions>
              </member>
            </Metrics>
            <NextToken>token-2</NextToken>
          </ListMetricsResult>
        </ListMetricsResponse>
        """;

    private const string LastPage =
        """
        <ListMetricsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
          <ListMetricsResult>
            <Metrics>
              <member>
                <Namespace>AWS/ELB</Namespace>
                <MetricName>RequestCount</MetricName>
                <Dimensions />
              </member>
            </Metrics>
          </ListMetricsResult>
        </ListMetricsResponse>
        """;

    [Fact]
    public async Task SendsOnlyActionAndVersionByDefault()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(OneMetric));

        await FakeHttp.CreateClient(handler).ListMetricsAsyncAsync(new ListMetricsRequest());

        Assert.Equal([new("Action", "ListMetrics"), new("Version", "2010-08-01")], handler.Request.Form);
    }

    [Fact]
    public async Task SendsFiltersAndNextToken()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(OneMetric));

        await FakeHttp.CreateClient(handler).ListMetricsAsyncAsync(new ListMetricsRequest {
            MetricName = "Latency",
            Namespace = "AWS/ELB",
            NextToken = "token/1+="
        });

        Assert.Equal("Latency", handler.Request["MetricName"]);
        Assert.Equal("AWS/ELB", handler.Request["Namespace"]);
        Assert.Equal("token/1+=", handler.Request["NextToken"]);
    }

    [Fact]
    public async Task ReturnsMetricsWithAllDimensions()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(OneMetric));

        var metrics = await FakeHttp.CreateClient(handler).ListMetricsAsyncAsync(new ListMetricsRequest());

        var metric = Assert.Single(metrics);

        Assert.Equal("AWS/ELB", metric.Namespace);
        Assert.Equal("Latency", metric.MetricName);
        Assert.Equal(2, metric.Dimensions.Count);
        Assert.Equal("AvailabilityZone", metric.Dimensions[1].Name);
        Assert.Equal("us-east-1a", metric.Dimensions[1].Value);
    }

    // The API reference says up to 500 results are returned per call, with a token for the rest
    [Fact]
    public async Task ExposesNextTokenForPaging()
    {
        var pages = new Queue<string>([OneMetric, LastPage]);
        var handler = new FakeHandler(() => FakeHttp.Xml(pages.Dequeue()));
        var client = FakeHttp.CreateClient(handler);

        var first = await client.ListMetricsAsync(new ListMetricsRequest());

        Assert.Equal("Latency", Assert.Single(first.Metrics).MetricName);
        Assert.Equal("token-2", first.NextToken);

        var second = await client.ListMetricsAsync(new ListMetricsRequest { NextToken = first.NextToken });

        Assert.Equal("RequestCount", Assert.Single(second.Metrics).MetricName);
        Assert.Null(second.NextToken);

        Assert.Null(handler.Requests[0]["NextToken"]);
        Assert.Equal("token-2", handler.Requests[1]["NextToken"]);
    }

    [Fact]
    public void ParsesEmptyMetrics()
    {
        var metrics = ListMetricsResponse.Parse(
            """
            <ListMetricsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <ListMetricsResult>
                <Metrics />
              </ListMetricsResult>
            </ListMetricsResponse>
            """u8.ToArray());

        Assert.Empty(metrics);
    }

    [Fact]
    public void ParsesMetricWithoutDimensions()
    {
        var metrics = ListMetricsResponse.Parse(
            """
            <ListMetricsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <ListMetricsResult>
                <Metrics>
                  <member>
                    <Namespace>Custom</Namespace>
                    <MetricName>Total</MetricName>
                    <Dimensions />
                  </member>
                </Metrics>
              </ListMetricsResult>
            </ListMetricsResponse>
            """u8.ToArray());

        Assert.Empty(Assert.Single(metrics).Dimensions);
    }

    [Fact]
    public async Task ThrowsOnErrorResponse()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml("<ErrorResponse><Error><Code>Throttling</Code></Error></ErrorResponse>", HttpStatusCode.BadRequest));

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => FakeHttp.CreateClient(handler).ListMetricsAsyncAsync(new ListMetricsRequest()));

        Assert.Contains("Throttling", ex.Message);
    }
}
