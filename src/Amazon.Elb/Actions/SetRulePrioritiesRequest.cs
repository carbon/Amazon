namespace Amazon.Elb;

public sealed class SetRulePrioritiesRequest : IElbRequest
{
    public SetRulePrioritiesRequest(RulePriorityPair[] rulePriorities)
    {
        ArgumentNullException.ThrowIfNull(rulePriorities);

        RulePriorities = rulePriorities;
    }

    public string Action => "SetRulePriorities";

    public RulePriorityPair[] RulePriorities { get; }
}