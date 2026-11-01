using System.Net;
using System.Net.Http;

namespace Amazon.CloudWatch.Tests;

public class GetMetricStatisticsRequestTests
{
    private const string EmptyResponse =
        """
        <GetMetricStatisticsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
          <GetMetricStatisticsResult>
            <Datapoints />
            <Label>CPUUtilization</Label>
          </GetMetricStatisticsResult>
        </GetMetricStatisticsResponse>
        """;

    private static async Task<RecordedRequest> SendAsync(GetMetricStatisticsRequest request)
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(EmptyResponse));

        await FakeHttp.CreateClient(handler).GetMetricStatisticsAsync(request);

        return handler.Request;
    }

    [Theory]
    [InlineData(null, "name")]
    [InlineData("", "name")]
    [InlineData("namespace", null)]
    [InlineData("namespace", "")]
    public void RejectsMissingNamespaceOrName(string? nameSpace, string? metricName)
    {
        Assert.ThrowsAny<ArgumentException>(() => new GetMetricStatisticsRequest(nameSpace!, metricName!));
    }

    [Fact]
    public async Task EncodesRequiredParameters()
    {
        var sent = await SendAsync(new GetMetricStatisticsRequest("AWS/EC2", "CPUUtilization") {
            StartTime = new DateTime(2016, 10, 03, 23, 00, 00, DateTimeKind.Utc),
            EndTime = new DateTime(2016, 10, 10, 23, 00, 00, DateTimeKind.Utc)
        });

        Assert.Equal([
            new("Action", "GetMetricStatistics"),
            new("Namespace", "AWS/EC2"),
            new("MetricName", "CPUUtilization"),
            new("StartTime", "2016-10-03T23:00:00Z"),
            new("EndTime", "2016-10-10T23:00:00Z"),
            new("Period", "60"),
            new("Version", "2010-08-01")
        ], sent.Form);
    }

    [Fact]
    public async Task EncodesPeriodInWholeSeconds()
    {
        var sent = await SendAsync(new GetMetricStatisticsRequest("N", "M") { Period = TimeSpan.FromHours(1) });

        Assert.Equal("3600", sent["Period"]);
    }

    [Fact]
    public async Task EncodesDimensionsFromOne()
    {
        var sent = await SendAsync(new GetMetricStatisticsRequest("N", "M") {
            Dimensions = [
                new Dimension { Name = "InstanceId", Value = "i-1" },
                new Dimension { Name = "AutoScalingGroupName", Value = "my group" }
            ]
        });

        Assert.Equal("InstanceId", sent["Dimensions.member.1.Name"]);
        Assert.Equal("i-1", sent["Dimensions.member.1.Value"]);
        Assert.Equal("AutoScalingGroupName", sent["Dimensions.member.2.Name"]);
        Assert.Equal("my group", sent["Dimensions.member.2.Value"]);
    }

    [Fact]
    public async Task EncodesEveryStatistic()
    {
        var sent = await SendAsync(new GetMetricStatisticsRequest("N", "M") {
            Statistics = [Statistic.SampleCount, Statistic.Average, Statistic.Sum, Statistic.Minimum, Statistic.Maximum]
        });

        Assert.Equal("SampleCount", sent["Statistics.member.1"]);
        Assert.Equal("Average", sent["Statistics.member.2"]);
        Assert.Equal("Sum", sent["Statistics.member.3"]);
        Assert.Equal("Minimum", sent["Statistics.member.4"]);
        Assert.Equal("Maximum", sent["Statistics.member.5"]);
    }

    [Fact]
    public void StatisticToStringIsItsName()
    {
        Assert.Equal("Average", Statistic.Average.ToString());
    }

    [Fact]
    public async Task EncodesUnitOnlyWhenSet()
    {
        var with = await SendAsync(new GetMetricStatisticsRequest("N", "M") { Unit = "Bytes/Second" });
        var without = await SendAsync(new GetMetricStatisticsRequest("N", "M"));

        Assert.Equal("Bytes/Second", with["Unit"]);
        Assert.Null(without["Unit"]);
        Assert.DoesNotContain(without.Form, f => f.Key.StartsWith("Dimensions", StringComparison.Ordinal) || f.Key.StartsWith("Statistics", StringComparison.Ordinal));
    }

    // Only meaningful where the local time zone isn't UTC; run with TZ=America/New_York to exercise it
    [Fact]
    public async Task ConvertsLocalTimesToUtc()
    {
        var start = new DateTime(2016, 10, 03, 12, 00, 00, DateTimeKind.Local);
        var end = start.AddHours(1);

        var sent = await SendAsync(new GetMetricStatisticsRequest("N", "M") { StartTime = start, EndTime = end });

        Assert.Equal(start.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"), sent["StartTime"]);
        Assert.Equal(end.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"), sent["EndTime"]);
    }

    [Fact]
    public async Task SendsUnspecifiedTimesAsGiven()
    {
        var sent = await SendAsync(new GetMetricStatisticsRequest("N", "M") {
            StartTime = new DateTime(2016, 10, 03, 12, 00, 00, DateTimeKind.Unspecified)
        });

        Assert.Equal("2016-10-03T12:00:00Z", sent["StartTime"]);
    }

    [Fact]
    public async Task PostsSignedFormToRegionalEndpoint()
    {
        var sent = await SendAsync(new GetMetricStatisticsRequest("N", "M"));

        Assert.Equal(HttpMethod.Post, sent.Method);
        Assert.Equal("https://monitoring.us-east-1.amazonaws.com/", sent.RequestUri.ToString());
        Assert.Equal("application/x-www-form-urlencoded", sent.ContentType);
        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=AKIDEXAMPLE/", sent.Authorization);
        Assert.Contains("/us-east-1/monitoring/aws4_request", sent.Authorization);
    }

    [Fact]
    public async Task ReturnsParsedResponse()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(
            """
            <GetMetricStatisticsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <GetMetricStatisticsResult>
                <Datapoints>
                  <member>
                    <Sum>12</Sum>
                    <Unit>Count</Unit>
                    <Timestamp>2016-11-02T12:01:00Z</Timestamp>
                  </member>
                </Datapoints>
                <Label>Requests</Label>
              </GetMetricStatisticsResult>
            </GetMetricStatisticsResponse>
            """));

        var response = await FakeHttp.CreateClient(handler).GetMetricStatisticsAsync(new GetMetricStatisticsRequest("N", "M"));

        Assert.Equal("Requests", response.GetMetricStatisticsResult.Label);
        Assert.Equal(12d, Assert.Single(response.GetMetricStatisticsResult.Datapoints).Sum);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, "BadRequest")]
    [InlineData(HttpStatusCode.Forbidden, "Forbidden")]
    [InlineData(HttpStatusCode.InternalServerError, "InternalServerError")]
    public async Task ThrowsWithStatusAndBodyOnErrorResponse(HttpStatusCode status, string expectedStatus)
    {
        var handler = new FakeHandler(() => FakeHttp.Xml("<ErrorResponse><Error><Code>SomeCode</Code></Error></ErrorResponse>", status));

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => FakeHttp.CreateClient(handler).GetMetricStatisticsAsync(new GetMetricStatisticsRequest("N", "M")));

        Assert.Contains(expectedStatus, ex.Message);
        Assert.Contains("SomeCode", ex.Message);
    }
}
