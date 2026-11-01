using System.Net;

namespace Amazon.CloudWatch.Tests;

public class GetMetricDataTests
{
    private const string EmptyResponse =
        """
        <GetMetricDataResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
          <GetMetricDataResult>
            <MetricDataResults />
          </GetMetricDataResult>
        </GetMetricDataResponse>
        """;

    private static readonly DateTime s_start = new(2018, 02, 17, 11, 37, 12, DateTimeKind.Utc);
    private static readonly DateTime s_end = new(2018, 02, 17, 11, 50, 32, DateTimeKind.Utc);

    private static async Task<RecordedRequest> SendAsync(GetMetricDataRequest request)
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(EmptyResponse));

        await FakeHttp.CreateClient(handler).GetMetricDataAsync(request);

        return handler.Request;
    }

    private static MetricDataQuery Expression(string id, string expression) => new() { Id = id, Expression = expression };

    private static MetricStat NetworkOut(string group) => new() {
        Metric = new Metric {
            Namespace = "AWS/EC2",
            MetricName = "NetworkOut",
            Dimensions = [new Dimension { Name = "AutoScalingGroupName", Value = group }]
        },
        Period = 300,
        Stat = "SampleCount",
        Unit = "Bytes"
    };

    // API reference example 4, which is a JSON request, using the query protocol's parameter names
    [Fact]
    public async Task EncodesDocumentedMetricMathSample()
    {
        var sent = await SendAsync(new GetMetricDataRequest {
            StartTime = DateTimeOffset.FromUnixTimeSeconds(1518867432).UtcDateTime,
            EndTime = DateTimeOffset.FromUnixTimeSeconds(1518868232).UtcDateTime,
            MetricDataQueries = [
                new MetricDataQuery { Id = "e1", Expression = "m2 / m1", Label = "my-asg-B / my-asg-A" },
                new MetricDataQuery { Id = "m1", MetricStat = NetworkOut("my-asg-A"), ReturnData = false },
                new MetricDataQuery { Id = "m2", MetricStat = NetworkOut("my-asg-B"), ReturnData = false }
            ]
        });

        Assert.Equal([
            new("Action", "GetMetricData"),
            new("StartTime", "2018-02-17T11:37:12Z"),
            new("EndTime", "2018-02-17T11:50:32Z"),
            new("MetricDataQueries.member.1.Id", "e1"),
            new("MetricDataQueries.member.1.Expression", "m2 / m1"),
            new("MetricDataQueries.member.1.Label", "my-asg-B / my-asg-A"),
            new("MetricDataQueries.member.2.Id", "m1"),
            new("MetricDataQueries.member.2.MetricStat.Metric.Namespace", "AWS/EC2"),
            new("MetricDataQueries.member.2.MetricStat.Metric.MetricName", "NetworkOut"),
            new("MetricDataQueries.member.2.MetricStat.Metric.Dimensions.member.1.Name", "AutoScalingGroupName"),
            new("MetricDataQueries.member.2.MetricStat.Metric.Dimensions.member.1.Value", "my-asg-A"),
            new("MetricDataQueries.member.2.MetricStat.Period", "300"),
            new("MetricDataQueries.member.2.MetricStat.Stat", "SampleCount"),
            new("MetricDataQueries.member.2.MetricStat.Unit", "Bytes"),
            new("MetricDataQueries.member.2.ReturnData", "false"),
            new("MetricDataQueries.member.3.Id", "m2"),
            new("MetricDataQueries.member.3.MetricStat.Metric.Namespace", "AWS/EC2"),
            new("MetricDataQueries.member.3.MetricStat.Metric.MetricName", "NetworkOut"),
            new("MetricDataQueries.member.3.MetricStat.Metric.Dimensions.member.1.Name", "AutoScalingGroupName"),
            new("MetricDataQueries.member.3.MetricStat.Metric.Dimensions.member.1.Value", "my-asg-B"),
            new("MetricDataQueries.member.3.MetricStat.Period", "300"),
            new("MetricDataQueries.member.3.MetricStat.Stat", "SampleCount"),
            new("MetricDataQueries.member.3.MetricStat.Unit", "Bytes"),
            new("MetricDataQueries.member.3.ReturnData", "false"),
            new("Version", "2010-08-01")
        ], sent.Form);
    }

    [Fact]
    public async Task EncodesOptionalParameters()
    {
        var sent = await SendAsync(new GetMetricDataRequest {
            StartTime = s_start,
            EndTime = s_end,
            LabelTimezone = "-0400",
            MaxDatapoints = 1000,
            NextToken = "token/1+=",
            ScanBy = "TimestampAscending",
            MetricDataQueries = [
                new MetricDataQuery { Id = "q1", Expression = "SELECT AVG(CPUUtilization) FROM SCHEMA(\"AWS/EC2\", InstanceId)", Period = 300, AccountId = "111111111111", ReturnData = true }
            ]
        });

        Assert.Equal("-0400", sent["LabelOptions.Timezone"]);
        Assert.Equal("1000", sent["MaxDatapoints"]);
        Assert.Equal("token/1+=", sent["NextToken"]);
        Assert.Equal("TimestampAscending", sent["ScanBy"]);
        Assert.Equal("SELECT AVG(CPUUtilization) FROM SCHEMA(\"AWS/EC2\", InstanceId)", sent["MetricDataQueries.member.1.Expression"]);
        Assert.Equal("300", sent["MetricDataQueries.member.1.Period"]);
        Assert.Equal("111111111111", sent["MetricDataQueries.member.1.AccountId"]);
        Assert.Equal("true", sent["MetricDataQueries.member.1.ReturnData"]);
    }

    [Fact]
    public async Task OmitsUnsetOptionalParameters()
    {
        var sent = await SendAsync(new GetMetricDataRequest { StartTime = s_start, EndTime = s_end, MetricDataQueries = [Expression("e1", "1")] });

        Assert.Equal(
            ["Action", "StartTime", "EndTime", "MetricDataQueries.member.1.Id", "MetricDataQueries.member.1.Expression", "Version"],
            sent.Form.Select(f => f.Key));
    }

    [Fact]
    public async Task EncodesMetricWithoutDimensions()
    {
        var sent = await SendAsync(new GetMetricDataRequest {
            StartTime = s_start,
            EndTime = s_end,
            MetricDataQueries = [
                new MetricDataQuery {
                    Id = "m1",
                    MetricStat = new MetricStat {
                        Metric = new Metric { Namespace = "MyService", MetricName = "ConnectionsFailed" },
                        Period = 60,
                        Stat = "Sum"
                    }
                }
            ]
        });

        Assert.Equal("ConnectionsFailed", sent["MetricDataQueries.member.1.MetricStat.Metric.MetricName"]);
        Assert.DoesNotContain(sent.Form, f => f.Key.Contains("Dimensions") || f.Key.EndsWith(".Unit", StringComparison.Ordinal));
    }

    // Only meaningful where the local time zone isn't UTC; run with TZ=America/New_York to exercise it
    [Fact]
    public async Task ConvertsLocalTimesToUtc()
    {
        var start = new DateTime(2018, 02, 17, 12, 00, 00, DateTimeKind.Local);

        var sent = await SendAsync(new GetMetricDataRequest { StartTime = start, EndTime = start.AddHours(1), MetricDataQueries = [Expression("e1", "1")] });

        Assert.Equal(start.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"), sent["StartTime"]);
        Assert.Equal(start.AddHours(1).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"), sent["EndTime"]);
    }

    // The values of API reference example 4's JSON response, in the query protocol's XML shape
    [Fact]
    public async Task ParsesResultsWithAndWithoutData()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(
            """
            <GetMetricDataResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <GetMetricDataResult>
                <MetricDataResults>
                  <member>
                    <Id>m1</Id>
                    <StatusCode>Complete</StatusCode>
                    <Timestamps />
                    <Values />
                  </member>
                  <member>
                    <Id>e1</Id>
                    <Label>my-asg-B / my-asg-A</Label>
                    <StatusCode>Complete</StatusCode>
                    <Timestamps>
                      <member>2018-02-17T11:47:12Z</member>
                      <member>2018-02-17T11:42:12Z</member>
                      <member>2018-02-17T11:37:12Z</member>
                    </Timestamps>
                    <Values>
                      <member>100.0</member>
                      <member>1.2158469945359334</member>
                      <member>2.0E7</member>
                    </Values>
                  </member>
                </MetricDataResults>
              </GetMetricDataResult>
              <ResponseMetadata>
                <RequestId>c919ffb2-a13d-11e6-994f-1d1bb7ed25dd</RequestId>
              </ResponseMetadata>
            </GetMetricDataResponse>
            """));

        var result = await FakeHttp.CreateClient(handler).GetMetricDataAsync(new GetMetricDataRequest { StartTime = s_start, EndTime = s_end, MetricDataQueries = [Expression("e1", "1")] });

        Assert.Null(result.NextToken);
        Assert.Equal(2, result.MetricDataResults.Count);

        var empty = result.MetricDataResults[0];

        Assert.Equal("m1", empty.Id);
        Assert.Empty(empty.Timestamps);
        Assert.Empty(empty.Values);

        var expression = result.MetricDataResults[1];

        Assert.Equal("my-asg-B / my-asg-A", expression.Label);
        Assert.Equal("Complete", expression.StatusCode);
        Assert.Equal([100d, 1.2158469945359334d, 2.0E7d], expression.Values);
        Assert.Equal(
            [1518868032, 1518867732, 1518867432],
            expression.Timestamps.Select(t => new DateTimeOffset(t).ToUnixTimeSeconds()));
        Assert.All(expression.Timestamps, t => Assert.Equal(DateTimeKind.Utc, t.Kind));
    }

    [Fact]
    public void ParsesMessages()
    {
        var result = GetMetricDataResponse.Deserialize(
            """
            <GetMetricDataResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <GetMetricDataResult>
                <Messages>
                  <member>
                    <Code>MaxMetricsExceeded</Code>
                    <Value>Maximum number of allowed metrics exceeded</Value>
                  </member>
                </Messages>
                <MetricDataResults>
                  <member>
                    <Id>m1</Id>
                    <StatusCode>PartialData</StatusCode>
                    <Messages>
                      <member>
                        <Code>ArithmeticError</Code>
                        <Value>One or more data-points have been dropped due to non-numeric values (NaN, -Infinite, +Infinite)</Value>
                      </member>
                    </Messages>
                  </member>
                </MetricDataResults>
                <NextToken>token-2</NextToken>
              </GetMetricDataResult>
            </GetMetricDataResponse>
            """u8.ToArray()).GetMetricDataResult;

        Assert.Equal("Maximum number of allowed metrics exceeded", Assert.Single(result.Messages).Value);
        Assert.Equal("token-2", result.NextToken);

        var metric = Assert.Single(result.MetricDataResults);

        Assert.Equal("PartialData", metric.StatusCode);
        Assert.Equal("ArithmeticError", Assert.Single(metric.Messages).Code);
    }

    [Fact]
    public async Task FollowsNextToken()
    {
        var pages = new Queue<string>([
            """
            <GetMetricDataResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <GetMetricDataResult>
                <MetricDataResults />
                <NextToken>token-2</NextToken>
              </GetMetricDataResult>
            </GetMetricDataResponse>
            """,
            EmptyResponse
        ]);

        var handler = new FakeHandler(() => FakeHttp.Xml(pages.Dequeue()));
        var client = FakeHttp.CreateClient(handler);

        var first = await client.GetMetricDataAsync(new GetMetricDataRequest { StartTime = s_start, EndTime = s_end, MetricDataQueries = [Expression("e1", "1")] });
        var second = await client.GetMetricDataAsync(new GetMetricDataRequest { StartTime = s_start, EndTime = s_end, MetricDataQueries = [Expression("e1", "1")], NextToken = first.NextToken });

        Assert.Equal("token-2", first.NextToken);
        Assert.Null(second.NextToken);
        Assert.Null(handler.Requests[0]["NextToken"]);
        Assert.Equal("token-2", handler.Requests[1]["NextToken"]);
    }

    [Fact]
    public async Task ThrowsOnErrorResponse()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml("<ErrorResponse><Error><Code>InvalidNextToken</Code></Error></ErrorResponse>", HttpStatusCode.BadRequest));

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => FakeHttp.CreateClient(handler).GetMetricDataAsync(new GetMetricDataRequest { StartTime = s_start, EndTime = s_end, MetricDataQueries = [Expression("e1", "1")] }));

        Assert.Contains("InvalidNextToken", ex.Message);
    }
}
