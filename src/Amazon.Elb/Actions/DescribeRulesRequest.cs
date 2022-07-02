#nullable disable

namespace Amazon.Elb;

public class DescribeRulesRequest : IElbRequest
{
    public string Action => "DescribeRules";

    public string ListenerArn { get; init; }
    
    public string[] RuleArns { get; init; }
}
