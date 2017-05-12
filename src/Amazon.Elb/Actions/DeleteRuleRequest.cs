using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class DeleteRuleRequest : IElbRequest
    {
        public string Action => "DeleteRule";

        [Required]
        public string RuleArn { get; set; }
    }
}