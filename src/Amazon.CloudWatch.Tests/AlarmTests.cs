using System.Globalization;
using System.Net;

namespace Amazon.CloudWatch.Tests;

public class AlarmTests
{
    private static async Task<RecordedRequest> PutAsync(PutMetricAlarmRequest request)
    {
        // Per the API reference, a successful call returns HTTP 200 with an empty body
        var handler = new FakeHandler(() => FakeHttp.Xml(""));

        await FakeHttp.CreateClient(handler).PutMetricAlarmAsync(request);

        return handler.Request;
    }

    private static MetricDataQuery Sum(string id, string metricName) => new() {
        Id = id,
        MetricStat = new MetricStat {
            Metric = new Metric { Namespace = "MyService", MetricName = metricName },
            Period = 60,
            Stat = "Sum"
        },
        ReturnData = false
    };

    // API reference "Static Threshold Alarm", which is a JSON request, using the query protocol's parameter names
    [Fact]
    public async Task EncodesDocumentedStaticThresholdSample()
    {
        var sent = await PutAsync(new PutMetricAlarmRequest {
            AlarmName = "Instance i-1234567890abcdef0 CPU Utilization",
            AlarmDescription = "CPU Utilization of i-1234567890abcdef0 with 40% as threshold",
            Namespace = "AWS/EC2",
            MetricName = "CPUUtilization",
            Dimensions = [new Dimension { Name = "InstanceId", Value = "i-1234567890abcdef0" }],
            AlarmActions = ["arn:aws:sns:us-west-1:123456789012:my_sns_topic"],
            ComparisonOperator = ComparisonOperator.GreaterThanThreshold,
            DatapointsToAlarm = 3,
            EvaluationPeriods = 4,
            Period = 60,
            Statistic = Statistic.Average,
            Threshold = 40
        });

        Assert.Equal([
            new("Action", "PutMetricAlarm"),
            new("AlarmName", "Instance i-1234567890abcdef0 CPU Utilization"),
            new("AlarmDescription", "CPU Utilization of i-1234567890abcdef0 with 40% as threshold"),
            new("AlarmActions.member.1", "arn:aws:sns:us-west-1:123456789012:my_sns_topic"),
            new("ComparisonOperator", "GreaterThanThreshold"),
            new("DatapointsToAlarm", "3"),
            new("EvaluationPeriods", "4"),
            new("Threshold", "40"),
            new("Namespace", "AWS/EC2"),
            new("MetricName", "CPUUtilization"),
            new("Dimensions.member.1.Name", "InstanceId"),
            new("Dimensions.member.1.Value", "i-1234567890abcdef0"),
            new("Period", "60"),
            new("Statistic", "Average"),
            new("Version", "2010-08-01")
        ], sent.Form);
    }

    // API reference "Metric Math Function Alarm", shortened to two metrics
    [Fact]
    public async Task EncodesDocumentedMetricMathSample()
    {
        var sent = await PutAsync(new PutMetricAlarmRequest {
            AlarmName = "MyService Connection Error Rate",
            AlarmActions = [
                "arn:aws:sns:us-west-1:123456789012:my_sns_topic",
                "arn:aws:sns:us-west-1:123456789012:my_other_sns_topic"
            ],
            ComparisonOperator = ComparisonOperator.GreaterThanThreshold,
            EvaluationPeriods = 3,
            Threshold = 40,
            Metrics = [
                Sum("m1", "ConnectionsFailed"),
                Sum("m4", "ConnectionAttempts"),
                new MetricDataQuery { Id = "error_rate", Expression = "(m1/m4)*100", ReturnData = true, Label = "Total Connection Error Rate" }
            ]
        });

        Assert.Equal("arn:aws:sns:us-west-1:123456789012:my_other_sns_topic", sent["AlarmActions.member.2"]);
        Assert.Equal("m1", sent["Metrics.member.1.Id"]);
        Assert.Equal("ConnectionsFailed", sent["Metrics.member.1.MetricStat.Metric.MetricName"]);
        Assert.Equal("MyService", sent["Metrics.member.1.MetricStat.Metric.Namespace"]);
        Assert.Equal("60", sent["Metrics.member.1.MetricStat.Period"]);
        Assert.Equal("Sum", sent["Metrics.member.1.MetricStat.Stat"]);
        Assert.Equal("false", sent["Metrics.member.1.ReturnData"]);
        Assert.Equal("(m1/m4)*100", sent["Metrics.member.3.Expression"]);
        Assert.Equal("true", sent["Metrics.member.3.ReturnData"]);
        Assert.Equal("Total Connection Error Rate", sent["Metrics.member.3.Label"]);
        Assert.Null(sent["MetricName"]);
        Assert.Null(sent["Period"]);
    }

    [Fact]
    public async Task SendsOnlyNameWhenNothingElseIsSet()
    {
        var sent = await PutAsync(new PutMetricAlarmRequest { AlarmName = "a" });

        Assert.Equal([new("Action", "PutMetricAlarm"), new("AlarmName", "a"), new("Version", "2010-08-01")], sent.Form);
    }

    [Fact]
    public async Task EncodesRemainingOptionalParameters()
    {
        var sent = await PutAsync(new PutMetricAlarmRequest {
            AlarmName = "a",
            ActionsEnabled = false,
            InsufficientDataActions = ["arn:insufficient"],
            OKActions = ["arn:ok-1", "arn:ok-2"],
            ThresholdMetricId = "t1",
            TreatMissingData = "notBreaching",
            EvaluateLowSampleCountPercentile = "ignore",
            ExtendedStatistic = "p90",
            Unit = "Percent",
            Tags = [new AlarmTag { Key = "team", Value = "core" }, new AlarmTag { Key = "env", Value = "prod" }]
        });

        Assert.Equal("false", sent["ActionsEnabled"]);
        Assert.Equal("arn:insufficient", sent["InsufficientDataActions.member.1"]);
        Assert.Equal("arn:ok-2", sent["OKActions.member.2"]);
        Assert.Equal("t1", sent["ThresholdMetricId"]);
        Assert.Equal("notBreaching", sent["TreatMissingData"]);
        Assert.Equal("ignore", sent["EvaluateLowSampleCountPercentile"]);
        Assert.Equal("p90", sent["ExtendedStatistic"]);
        Assert.Equal("Percent", sent["Unit"]);
        Assert.Equal("team", sent["Tags.member.1.Key"]);
        Assert.Equal("core", sent["Tags.member.1.Value"]);
        Assert.Equal("env", sent["Tags.member.2.Key"]);
        Assert.Equal("prod", sent["Tags.member.2.Value"]);
    }

    [Fact]
    public async Task SendsZeroThreshold()
    {
        var sent = await PutAsync(new PutMetricAlarmRequest { AlarmName = "a", Threshold = 0 });

        Assert.Equal("0", sent["Threshold"]);
    }

    [Fact]
    public async Task FormatsThresholdWithInvariantCulture()
    {
        var original = CultureInfo.CurrentCulture;

        // Built by hand since the container may run in invariant globalization mode
        var commaDecimal = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        commaDecimal.NumberFormat.NumberDecimalSeparator = ",";

        CultureInfo.CurrentCulture = commaDecimal;

        try
        {
            var sent = await PutAsync(new PutMetricAlarmRequest { AlarmName = "a", Threshold = 1.5 });

            Assert.Equal("1.5", sent["Threshold"]);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public async Task PutThrowsOnErrorResponse()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml("<ErrorResponse><Error><Code>LimitExceeded</Code></Error></ErrorResponse>", HttpStatusCode.BadRequest));

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => FakeHttp.CreateClient(handler).PutMetricAlarmAsync(new PutMetricAlarmRequest { AlarmName = "a" }));

        Assert.Contains("LimitExceeded", ex.Message);
    }

    private const string TwoAlarmsPage =
        """
        <DescribeAlarmsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
          <DescribeAlarmsResult>
            <MetricAlarms>
              <member>
                <AlarmName>cpu-high</AlarmName>
                <AlarmArn>arn:aws:cloudwatch:us-east-1:123456789012:alarm:cpu-high</AlarmArn>
                <AlarmDescription>CPU over 40%</AlarmDescription>
                <AlarmConfigurationUpdatedTimestamp>2024-01-15T12:00:00.123Z</AlarmConfigurationUpdatedTimestamp>
                <ActionsEnabled>true</ActionsEnabled>
                <AlarmActions>
                  <member>arn:aws:sns:us-east-1:123456789012:alarm</member>
                </AlarmActions>
                <OKActions>
                  <member>arn:aws:sns:us-east-1:123456789012:ok</member>
                </OKActions>
                <InsufficientDataActions />
                <StateValue>ALARM</StateValue>
                <StateReason>Threshold Crossed</StateReason>
                <StateReasonData>{"version":"1.0"}</StateReasonData>
                <StateUpdatedTimestamp>2024-01-15T12:05:00Z</StateUpdatedTimestamp>
                <StateTransitionedTimestamp>2024-01-15T12:04:00Z</StateTransitionedTimestamp>
                <MetricName>CPUUtilization</MetricName>
                <Namespace>AWS/EC2</Namespace>
                <Statistic>Average</Statistic>
                <Dimensions>
                  <member>
                    <Name>InstanceId</Name>
                    <Value>i-1234567890abcdef0</Value>
                  </member>
                </Dimensions>
                <Period>60</Period>
                <EvaluationPeriods>4</EvaluationPeriods>
                <DatapointsToAlarm>3</DatapointsToAlarm>
                <Threshold>40.5</Threshold>
                <ComparisonOperator>GreaterThanThreshold</ComparisonOperator>
                <TreatMissingData>notBreaching</TreatMissingData>
                <Unit>Percent</Unit>
              </member>
              <member>
                <AlarmName>error-rate</AlarmName>
                <ActionsEnabled>false</ActionsEnabled>
                <StateValue>INSUFFICIENT_DATA</StateValue>
                <EvaluationPeriods>3</EvaluationPeriods>
                <ComparisonOperator>LessThanLowerOrGreaterThanUpperThreshold</ComparisonOperator>
                <ThresholdMetricId>t1</ThresholdMetricId>
                <ExtendedStatistic>p90</ExtendedStatistic>
                <EvaluateLowSampleCountPercentile>ignore</EvaluateLowSampleCountPercentile>
                <EvaluationState>PARTIAL_DATA</EvaluationState>
                <Metrics>
                  <member>
                    <Id>m1</Id>
                    <MetricStat>
                      <Metric>
                        <Namespace>MyService</Namespace>
                        <MetricName>ConnectionsFailed</MetricName>
                        <Dimensions />
                      </Metric>
                      <Period>60</Period>
                      <Stat>Sum</Stat>
                    </MetricStat>
                    <ReturnData>false</ReturnData>
                  </member>
                  <member>
                    <Id>t1</Id>
                    <Expression>ANOMALY_DETECTION_BAND(m1, 3)</Expression>
                    <Label>Band</Label>
                    <Period>300</Period>
                    <AccountId>111111111111</AccountId>
                    <ReturnData>true</ReturnData>
                  </member>
                </Metrics>
              </member>
            </MetricAlarms>
            <CompositeAlarms />
            <NextToken>token-2</NextToken>
          </DescribeAlarmsResult>
          <ResponseMetadata>
            <RequestId>c919ffb2-a13d-11e6-994f-1d1bb7ed25dd</RequestId>
          </ResponseMetadata>
        </DescribeAlarmsResponse>
        """;

    private const string LastPage =
        """
        <DescribeAlarmsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
          <DescribeAlarmsResult>
            <MetricAlarms />
          </DescribeAlarmsResult>
        </DescribeAlarmsResponse>
        """;

    [Fact]
    public async Task DescribeSendsOnlyActionAndVersionByDefault()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(LastPage));

        var result = await FakeHttp.CreateClient(handler).DescribeAlarmsAsync(new DescribeAlarmsRequest());

        Assert.Equal([new("Action", "DescribeAlarms"), new("Version", "2010-08-01")], handler.Request.Form);
        Assert.Empty(result.MetricAlarms);
        Assert.Null(result.NextToken);
    }

    [Fact]
    public async Task DescribeEncodesFilters()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(LastPage));

        await FakeHttp.CreateClient(handler).DescribeAlarmsAsync(new DescribeAlarmsRequest {
            ActionPrefix = "arn:aws:sns:us-east-1:123456789012:alarm",
            AlarmNamePrefix = "cpu",
            AlarmNames = ["a", "b"],
            MaxRecords = 50,
            NextToken = "token/1+=",
            StateValue = AlarmStateValue.InsufficientData
        });

        var sent = handler.Request;

        Assert.Equal("arn:aws:sns:us-east-1:123456789012:alarm", sent["ActionPrefix"]);
        Assert.Equal("cpu", sent["AlarmNamePrefix"]);
        Assert.Equal("a", sent["AlarmNames.member.1"]);
        Assert.Equal("b", sent["AlarmNames.member.2"]);
        Assert.Equal("50", sent["MaxRecords"]);
        Assert.Equal("token/1+=", sent["NextToken"]);
        Assert.Equal("INSUFFICIENT_DATA", sent["StateValue"]);
    }

    [Fact]
    public void ParsesSingleMetricAlarm()
    {
        var alarm = DescribeAlarmsResponse.Deserialize(System.Text.Encoding.UTF8.GetBytes(TwoAlarmsPage)).DescribeAlarmsResult.MetricAlarms[0];

        Assert.Equal("cpu-high", alarm.AlarmName);
        Assert.Equal("arn:aws:cloudwatch:us-east-1:123456789012:alarm:cpu-high", alarm.AlarmArn);
        Assert.Equal("CPU over 40%", alarm.AlarmDescription);
        Assert.Equal(new DateTime(2024, 01, 15, 12, 00, 00, 123, DateTimeKind.Utc), alarm.AlarmConfigurationUpdatedTimestamp);
        Assert.True(alarm.ActionsEnabled);
        Assert.Equal("arn:aws:sns:us-east-1:123456789012:alarm", Assert.Single(alarm.AlarmActions));
        Assert.Equal("arn:aws:sns:us-east-1:123456789012:ok", Assert.Single(alarm.OKActions));
        Assert.Empty(alarm.InsufficientDataActions);
        Assert.Equal(AlarmStateValue.Alarm, alarm.StateValue);
        Assert.Equal("Threshold Crossed", alarm.StateReason);
        Assert.Equal("""{"version":"1.0"}""", alarm.StateReasonData);
        Assert.Equal(new DateTime(2024, 01, 15, 12, 05, 00, DateTimeKind.Utc), alarm.StateUpdatedTimestamp);
        Assert.Equal(DateTimeKind.Utc, alarm.StateUpdatedTimestamp.Value.Kind);
        Assert.Equal(new DateTime(2024, 01, 15, 12, 04, 00, DateTimeKind.Utc), alarm.StateTransitionedTimestamp);
        Assert.Equal("CPUUtilization", alarm.MetricName);
        Assert.Equal("AWS/EC2", alarm.Namespace);
        Assert.Equal("Average", alarm.Statistic);
        Assert.Equal("i-1234567890abcdef0", Assert.Single(alarm.Dimensions).Value);
        Assert.Equal(60, alarm.Period);
        Assert.Equal(4, alarm.EvaluationPeriods);
        Assert.Equal(3, alarm.DatapointsToAlarm);
        Assert.Equal(40.5, alarm.Threshold);
        Assert.Equal(ComparisonOperator.GreaterThanThreshold, alarm.ComparisonOperator);
        Assert.Equal("notBreaching", alarm.TreatMissingData);
        Assert.Equal("Percent", alarm.Unit);
    }

    [Fact]
    public void ParsesMetricMathAlarm()
    {
        var alarm = DescribeAlarmsResponse.Deserialize(System.Text.Encoding.UTF8.GetBytes(TwoAlarmsPage)).DescribeAlarmsResult.MetricAlarms[1];

        Assert.False(alarm.ActionsEnabled);
        Assert.Equal(AlarmStateValue.InsufficientData, alarm.StateValue);
        Assert.Equal(ComparisonOperator.LessThanLowerOrGreaterThanUpperThreshold, alarm.ComparisonOperator);
        Assert.Equal("t1", alarm.ThresholdMetricId);
        Assert.Equal("p90", alarm.ExtendedStatistic);
        Assert.Equal("ignore", alarm.EvaluateLowSampleCountPercentile);
        Assert.Equal("PARTIAL_DATA", alarm.EvaluationState);
        Assert.Null(alarm.Threshold);
        Assert.Null(alarm.Period);
        Assert.Null(alarm.DatapointsToAlarm);
        Assert.Null(alarm.AlarmConfigurationUpdatedTimestamp);
        Assert.Equal(2, alarm.Metrics.Count);

        var metric = alarm.Metrics[0];

        Assert.Equal("m1", metric.Id);
        Assert.Equal("ConnectionsFailed", metric.MetricStat!.Metric.MetricName);
        Assert.Equal("MyService", metric.MetricStat.Metric.Namespace);
        Assert.Empty(metric.MetricStat.Metric.Dimensions);
        Assert.Equal(60, metric.MetricStat.Period);
        Assert.Equal("Sum", metric.MetricStat.Stat);
        Assert.False(metric.ReturnData);
        Assert.Null(metric.Expression);

        var expression = alarm.Metrics[1];

        Assert.Equal("ANOMALY_DETECTION_BAND(m1, 3)", expression.Expression);
        Assert.Equal("Band", expression.Label);
        Assert.Equal(300, expression.Period);
        Assert.Equal("111111111111", expression.AccountId);
        Assert.True(expression.ReturnData);
        Assert.Null(expression.MetricStat);
    }

    [Fact]
    public void MissingDimensionsParseAsEmpty()
    {
        var metrics = ListMetricsResponse.Parse(
            """
            <ListMetricsResponse xmlns="http://monitoring.amazonaws.com/doc/2010-08-01/">
              <ListMetricsResult>
                <Metrics>
                  <member>
                    <Namespace>Custom</Namespace>
                    <MetricName>Total</MetricName>
                  </member>
                </Metrics>
              </ListMetricsResult>
            </ListMetricsResponse>
            """u8.ToArray());

        Assert.Empty(Assert.Single(metrics).Dimensions);
    }

    [Fact]
    public async Task DescribeFollowsNextToken()
    {
        var pages = new Queue<string>([TwoAlarmsPage, LastPage]);
        var handler = new FakeHandler(() => FakeHttp.Xml(pages.Dequeue()));
        var client = FakeHttp.CreateClient(handler);

        var first = await client.DescribeAlarmsAsync(new DescribeAlarmsRequest());
        var second = await client.DescribeAlarmsAsync(new DescribeAlarmsRequest { NextToken = first.NextToken });

        Assert.Equal(2, first.MetricAlarms.Count);
        Assert.Equal("token-2", first.NextToken);
        Assert.Null(second.NextToken);
        Assert.Null(handler.Requests[0]["NextToken"]);
        Assert.Equal("token-2", handler.Requests[1]["NextToken"]);
    }

    public static TheoryData<string, Func<CloudWatchClient, IReadOnlyList<string>, Task>> AlarmNameActions => new() {
        { "DeleteAlarms", (client, names) => client.DeleteAlarmsAsync(names) },
        { "EnableAlarmActions", (client, names) => client.EnableAlarmActionsAsync(names) },
        { "DisableAlarmActions", (client, names) => client.DisableAlarmActionsAsync(names) }
    };

    [Theory]
    [MemberData(nameof(AlarmNameActions))]
    public async Task EncodesAlarmNames(string action, Func<CloudWatchClient, IReadOnlyList<string>, Task> call)
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(""));

        await call(FakeHttp.CreateClient(handler), ["first alarm", "second"]);

        Assert.Equal([
            new("Action", action),
            new("AlarmNames.member.1", "first alarm"),
            new("AlarmNames.member.2", "second"),
            new("Version", "2010-08-01")
        ], handler.Request.Form);
    }

    [Theory]
    [MemberData(nameof(AlarmNameActions))]
    public async Task RejectsMissingAlarmNamesWithoutSending(string action, Func<CloudWatchClient, IReadOnlyList<string>, Task> call)
    {
        Assert.NotEmpty(action);

        var handler = new FakeHandler(() => FakeHttp.Xml(""));
        var client = FakeHttp.CreateClient(handler);

        await Assert.ThrowsAnyAsync<ArgumentException>(() => call(client, []));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => call(client, null!));

        Assert.Empty(handler.Requests);
    }

    [Theory]
    [MemberData(nameof(AlarmNameActions))]
    public async Task AlarmNameActionsThrowOnErrorResponse(string action, Func<CloudWatchClient, IReadOnlyList<string>, Task> call)
    {
        Assert.NotEmpty(action);

        var handler = new FakeHandler(() => FakeHttp.Xml("<ErrorResponse><Error><Code>ResourceNotFound</Code></Error></ErrorResponse>", HttpStatusCode.NotFound));

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => call(FakeHttp.CreateClient(handler), ["a"]));

        Assert.Contains("ResourceNotFound", ex.Message);
    }

    // API reference example, with StateReasonData as the JSON string the parameter's type calls for
    [Fact]
    public async Task EncodesDocumentedSetAlarmStateSample()
    {
        const string reasonData = """{"Version":"1.0","Statistic":"Average","Period":60,"RecentDatapoints":[100],"Threshold":50}""";

        var handler = new FakeHandler(() => FakeHttp.Xml(""));

        await FakeHttp.CreateClient(handler).SetAlarmStateAsync(new SetAlarmStateRequest {
            AlarmName = "ExampleAlarmName",
            StateValue = AlarmStateValue.Alarm,
            StateReason = "Testing Alarm State",
            StateReasonData = reasonData
        });

        Assert.Equal([
            new("Action", "SetAlarmState"),
            new("AlarmName", "ExampleAlarmName"),
            new("StateValue", "ALARM"),
            new("StateReason", "Testing Alarm State"),
            new("StateReasonData", reasonData),
            new("Version", "2010-08-01")
        ], handler.Request.Form);
    }

    [Fact]
    public async Task SetAlarmStateOmitsUnsetReasonData()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml(""));

        await FakeHttp.CreateClient(handler).SetAlarmStateAsync(new SetAlarmStateRequest {
            AlarmName = "a",
            StateValue = AlarmStateValue.OK,
            StateReason = "reset"
        });

        Assert.Equal("OK", handler.Request["StateValue"]);
        Assert.Null(handler.Request["StateReasonData"]);
    }
}
