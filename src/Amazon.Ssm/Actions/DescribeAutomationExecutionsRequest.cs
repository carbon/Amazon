namespace Amazon.Ssm;

public sealed class DescribeAutomationExecutionsRequest
{
    public AutomationExecutionFilter[]? Filters { get; set; }

    public int? MaxResults { get; set; }

    public string? NextToken { get; set; }
}

public sealed class AutomationExecutionFilter
{
    public AutomationExecutionFilter(string key, string[] values)
    {
        Key = key;
        Values = values;
    }

    public string Key { get; }

    public string[] Values { get; }
}