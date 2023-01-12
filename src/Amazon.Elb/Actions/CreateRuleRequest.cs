using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb;

public sealed class CreateRuleRequest : IElbRequest
{
    public string Action => "CreateRule";

    public required Action[] Actions { get; init; }

    public required RuleCondition[] Conditions { get; init; }

    public required string ListenerArn { get; init; }

    [Range(1, 99_999)]
    public required int Priority { get; init; }
}
