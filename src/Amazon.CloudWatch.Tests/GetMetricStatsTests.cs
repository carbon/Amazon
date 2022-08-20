namespace Amazon.CloudWatch.Tests;

public class GetMetricStatsTests
{
    [Fact]
    public void A()
    {        
        var result = GetMetricStatatisticsResponse.Deserialize(
            """
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
            """);

        Assert.Equal(3.895104895104895d, result.Datapoints[0].Average.Value);
        Assert.Equal(12, result.Datapoints.Count);
        Assert.Equal("HealthyHostCount", result.Label);

        var date = new DateTime(2016, 11, 02, 12, 01, 00, DateTimeKind.Utc);

        Assert.Equal(date, result.Datapoints[0].Timestamp);
        Assert.Equal(3.895104895104895, result.Datapoints[0].Average.Value);
    }
}
