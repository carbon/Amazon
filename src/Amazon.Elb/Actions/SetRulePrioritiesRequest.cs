using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class SetRulePrioritiesRequest : IElbRequest
    {
        public string Action => "SetRulePriorities";

        [Required]
        public RulePriorityPair[] RulePriorities { get; set; }
    }
}