#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public sealed class CreateRuleRequest : IElbRequest
    {
        public string Action => "CreateRule";

        public Action[] Actions { get; init; }

        public RuleCondition[] Conditions { get; init; }

        [Required]
        public string ListenerArn { get; init; }

        [Range(1, 99_999)]
        public int Priority { get; init; }
    }
}