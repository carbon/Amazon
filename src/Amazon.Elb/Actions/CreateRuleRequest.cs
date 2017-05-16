using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class CreateRuleRequest : IElbRequest
    {
        public string Action => "CreateRule";

        public Action[] Actions { get; set; }

        public RuleCondition[] Conditions { get; set; }

        [Required]
        public string ListenerArn { get; set; }

        [Range(1, 99999)]
        public int Priority { get; set; }
    }
}