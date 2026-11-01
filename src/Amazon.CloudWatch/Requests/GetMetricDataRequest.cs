using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

public sealed class GetMetricDataRequest
{
    public required IReadOnlyList<MetricDataQuery> MetricDataQueries { get; init; }

    public required DateTime StartTime { get; init; }

    public required DateTime EndTime { get; init; }

    /// <summary>
    /// The UTC offset, such as -0400, used to display times in dynamic labels.
    /// </summary>
    public string? LabelTimezone { get; init; }

    public int? MaxDatapoints { get; init; }

    public string? NextToken { get; init; }

    /// <summary>
    /// TimestampDescending (the default) or TimestampAscending.
    /// </summary>
    public string? ScanBy { get; init; }

    internal List<KeyValuePair<string, string>> ToParameters()
    {
        var parameters = new List<KeyValuePair<string, string>> {
            new("Action", "GetMetricData"),
            new("StartTime", CloudWatchTimestamp.Format(StartTime)),
            new("EndTime", CloudWatchTimestamp.Format(EndTime))
        };

        MetricDataQuery.AddAll(parameters, "MetricDataQueries", MetricDataQueries);

        parameters.AddOptional("LabelOptions.Timezone", LabelTimezone);
        parameters.AddOptional("MaxDatapoints", MaxDatapoints);
        parameters.AddOptional("NextToken", NextToken);
        parameters.AddOptional("ScanBy", ScanBy);

        return parameters;
    }
}
