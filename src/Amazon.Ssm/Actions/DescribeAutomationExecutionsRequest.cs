namespace Amazon.Ssm;

public sealed class DescribeAutomationExecutionsRequest
{
    public AutomationExecutionFilter[]? Filters { get; set; }

    public int? MaxResults { get; set; }

    public string? NextToken { get; set; }
}

public sealed class AutomationExecutionFilter(string key, string[] values)
{
    public string Key { get; } = key;

    public string[] Values { get; } = values;
}