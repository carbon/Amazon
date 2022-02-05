namespace Amazon.Ses;

public sealed class UpdateReceiptRuleRequest
{
    public UpdateReceiptRuleRequest(ReceiptRule rule, string ruleSetName)
    {
        Rule = rule;
        RuleSetName = ruleSetName;
    }

    public ReceiptRule Rule { get; }

    public string RuleSetName { get; }
}