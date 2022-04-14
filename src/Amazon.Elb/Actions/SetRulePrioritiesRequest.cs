using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb;

public sealed class SetRulePrioritiesRequest : IElbRequest
{
    public string Action => "SetRulePriorities";

    public SetRulePrioritiesRequest(RulePriorityPair[] rulePriorities!!)
    {
        RulePriorities = rulePriorities;
    }

    [Required]
    public RulePriorityPair[] RulePriorities { get; }
}