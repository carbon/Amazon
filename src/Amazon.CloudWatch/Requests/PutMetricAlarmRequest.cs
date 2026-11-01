using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

/// <summary>
/// Creates or replaces a metric alarm. An update overwrites the alarm's whole configuration, except for its tags.
/// </summary>
public sealed class PutMetricAlarmRequest
{
    public required string AlarmName { get; init; }

    public string? AlarmDescription { get; init; }

    public bool? ActionsEnabled { get; init; }

    public IReadOnlyList<string>? AlarmActions { get; init; }

    public IReadOnlyList<string>? InsufficientDataActions { get; init; }

    public IReadOnlyList<string>? OKActions { get; init; }

    public string? ComparisonOperator { get; init; }

    public int? DatapointsToAlarm { get; init; }

    public int? EvaluationPeriods { get; init; }

    public double? Threshold { get; init; }

    public string? ThresholdMetricId { get; init; }

    public string? TreatMissingData { get; init; }

    public string? EvaluateLowSampleCountPercentile { get; init; }

    public IReadOnlyList<AlarmTag>? Tags { get; init; }

    // A single metric. Use these or Metrics, but not both

    public string? Namespace { get; init; }

    public string? MetricName { get; init; }

    public IReadOnlyList<Dimension>? Dimensions { get; init; }

    /// <summary>
    /// The length, in seconds, used each time the metric is evaluated.
    /// </summary>
    public int? Period { get; init; }

    public Statistic? Statistic { get; init; }

    public string? ExtendedStatistic { get; init; }

    public string? Unit { get; init; }

    // A metric math expression, Metrics Insights query or anomaly detection model

    public IReadOnlyList<MetricDataQuery>? Metrics { get; init; }

    internal List<KeyValuePair<string, string>> ToParameters()
    {
        var parameters = new List<KeyValuePair<string, string>> {
            new("Action", "PutMetricAlarm"),
            new("AlarmName", AlarmName)
        };

        parameters.AddOptional("AlarmDescription", AlarmDescription);
        parameters.AddOptional("ActionsEnabled", ActionsEnabled);
        parameters.AddMembers("AlarmActions", AlarmActions);
        parameters.AddMembers("InsufficientDataActions", InsufficientDataActions);
        parameters.AddMembers("OKActions", OKActions);
        parameters.AddOptional("ComparisonOperator", ComparisonOperator);
        parameters.AddOptional("DatapointsToAlarm", DatapointsToAlarm);
        parameters.AddOptional("EvaluationPeriods", EvaluationPeriods);
        parameters.AddOptional("Threshold", Threshold);
        parameters.AddOptional("ThresholdMetricId", ThresholdMetricId);
        parameters.AddOptional("TreatMissingData", TreatMissingData);
        parameters.AddOptional("EvaluateLowSampleCountPercentile", EvaluateLowSampleCountPercentile);

        parameters.AddOptional("Namespace", Namespace);
        parameters.AddOptional("MetricName", MetricName);
        parameters.AddDimensions("Dimensions", Dimensions);
        parameters.AddOptional("Period", Period);
        parameters.AddOptional("Statistic", Statistic?.Name);
        parameters.AddOptional("ExtendedStatistic", ExtendedStatistic);
        parameters.AddOptional("Unit", Unit);

        MetricDataQuery.AddAll(parameters, "Metrics", Metrics);

        if (Tags != null)
        {
            for (int i = 0; i < Tags.Count; i++)
            {
                string prefix = CloudWatchParameters.Member("Tags", i);

                parameters.Add(new($"{prefix}.Key", Tags[i].Key));
                parameters.Add(new($"{prefix}.Value", Tags[i].Value));
            }
        }

        return parameters;
    }
}
