using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

public sealed class MetricStat
{
    public required Metric Metric { get; init; }

    /// <summary>
    /// The granularity, in seconds, of the returned data points.
    /// </summary>
    public required int Period { get; init; }

    /// <summary>
    /// The statistic to return, such as Average or p90.
    /// </summary>
    public required string Stat { get; init; }

    public string? Unit { get; init; }

    internal void AddTo(List<KeyValuePair<string, string>> parameters, string prefix)
    {
        parameters.Add(new($"{prefix}.Metric.Namespace", Metric.Namespace));
        parameters.Add(new($"{prefix}.Metric.MetricName", Metric.MetricName));
        parameters.AddDimensions($"{prefix}.Metric.Dimensions", Metric.Dimensions);
        parameters.AddOptional($"{prefix}.Period", Period);
        parameters.Add(new($"{prefix}.Stat", Stat));
        parameters.AddOptional($"{prefix}.Unit", Unit);
    }
}
