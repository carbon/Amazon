namespace Amazon.Ses;

public sealed class CreateReceiptRuleRequest
{
    public CreateReceiptRuleRequest(ReceiptRule rule, string ruleSetName, string? after = null)
    {
        Rule = rule;
        RuleSetName = ruleSetName;
        After = after;
    }

    public ReceiptRule Rule { get; }

    public string RuleSetName { get; }

    public string? After { get; }
}