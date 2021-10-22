namespace Amazon.Elb;

public sealed class DeleteRuleRequest : IElbRequest
{
    public DeleteRuleRequest(string ruleArn)
    {
        ArgumentNullException.ThrowIfNull(ruleArn);

        RuleArn = ruleArn;
    }

    public string Action => "DeleteRule";

    public string RuleArn { get; }
}