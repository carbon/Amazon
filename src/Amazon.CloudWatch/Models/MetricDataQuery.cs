using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

/// <summary>
/// A metric to retrieve or an expression to evaluate. Specify either <see cref="MetricStat"/> or <see cref="Expression"/>, but not both.
/// </summary>
public sealed class MetricDataQuery
{
    public required string Id { get; init; }

    public string? AccountId { get; init; }

    public string? Expression { get; init; }

    public string? Label { get; init; }

    public MetricStat? MetricStat { get; init; }

    /// <summary>
    /// The granularity, in seconds, of the returned data points.
    /// </summary>
    public int? Period { get; init; }

    public bool? ReturnData { get; init; }

    internal void AddTo(List<KeyValuePair<string, string>> parameters, string prefix)
    {
        parameters.Add(new($"{prefix}.Id", Id));
        parameters.AddOptional($"{prefix}.AccountId", AccountId);
        parameters.AddOptional($"{prefix}.Expression", Expression);
        parameters.AddOptional($"{prefix}.Label", Label);
        MetricStat?.AddTo(parameters, $"{prefix}.MetricStat");
        parameters.AddOptional($"{prefix}.Period", Period);
        parameters.AddOptional($"{prefix}.ReturnData", ReturnData);
    }

    internal static void AddAll(List<KeyValuePair<string, string>> parameters, string name, IReadOnlyList<MetricDataQuery>? queries)
    {
        if (queries is null) return;

        for (int i = 0; i < queries.Count; i++)
        {
            queries[i].AddTo(parameters, CloudWatchParameters.Member(name, i));
        }
    }
}
