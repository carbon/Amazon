#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb;

public sealed class ModifyRuleRequest : IElbRequest
{
    public string Action => "ModifyRule";

    public Action[] Actions { get; init; }

    public RuleCondition[] Conditions { get; init; }

    [Required]
    public string RuleArn { get; init; }
}