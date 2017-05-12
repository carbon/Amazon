using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class ModifyRuleRequest : IElbRequest
    {
        public string Action => "ModifyRule";

        public Action[] Actions { get; set; }

        public RuleCondition[] Conditions { get; set; }

        [Required]
        public string RuleArn { get; set; }
    }
}