namespace Amazon.Ses;

public sealed class UpdateReceiptRuleRequest(
    ReceiptRule rule,
    string ruleSetName)
{
    public ReceiptRule Rule { get; } = rule;

    public string RuleSetName { get; } = ruleSetName;
}