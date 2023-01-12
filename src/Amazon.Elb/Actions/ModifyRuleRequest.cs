namespace Amazon.Elb;

public sealed class ModifyRuleRequest : IElbRequest
{
    public string Action => "ModifyRule";

    public Action[]? Actions { get; init; }

    public RuleCondition[]? Conditions { get; init; }

    public required string RuleArn { get; init; }
}