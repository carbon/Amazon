using System;

namespace Amazon.Elb;

public sealed class DeleteRuleRequest : IElbRequest
{
    public string Action => "DeleteRule";

    public DeleteRuleRequest(string ruleArn)
    {
        RuleArn = ruleArn ?? throw new ArgumentNullException(nameof(ruleArn));
    }

    public string RuleArn { get; }
}