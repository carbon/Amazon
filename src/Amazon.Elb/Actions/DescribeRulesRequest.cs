namespace Amazon.Elb;

public sealed class DescribeRulesRequest : IElbRequest
{
    public string Action => "DescribeRules";

    public string? ListenerArn { get; init; }
    
    public string[]? RuleArns { get; init; }

    public string? Marker { get; init; }
}