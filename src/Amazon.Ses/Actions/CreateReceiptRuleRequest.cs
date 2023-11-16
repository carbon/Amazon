namespace Amazon.Ses;

public sealed class CreateReceiptRuleRequest(
    ReceiptRule rule,
    string ruleSetName,
    string? after = null)
{
    public ReceiptRule Rule { get; } = rule;

    public string RuleSetName { get; } = ruleSetName;

    public string? After { get; } = after;
}