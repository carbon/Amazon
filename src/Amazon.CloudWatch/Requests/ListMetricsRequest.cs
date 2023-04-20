namespace Amazon.CloudWatch;

public sealed class ListMetricsRequest
{
    public string? MetricName { get; set; }

    public string? Namespace { get; set; }

    public string? NextToken { get; set; }

    internal List<KeyValuePair<string, string>> ToParameters()
    {
        var parameters = new List<KeyValuePair<string, string>>(5) {
            new("Action", "ListMetrics")
        };

        if (MetricName != null) parameters.Add(new("MetricName", MetricName));
        if (Namespace != null)  parameters.Add(new("Namespace", Namespace));
        if (NextToken != null)  parameters.Add(new("NextToken", NextToken));

        return parameters;
    }
}
