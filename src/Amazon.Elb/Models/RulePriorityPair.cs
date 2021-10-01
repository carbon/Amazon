#nullable disable

namespace Amazon.Elb;

public sealed class RulePriorityPair
{
    public int? Priority { get; init; }

    public string RuleArn { get; init; }
}