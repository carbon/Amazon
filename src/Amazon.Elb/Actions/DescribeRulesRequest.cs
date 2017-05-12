using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class DescribeRulesRequest : IElbRequest
    {
        public string Action => "DescribeRules";

        public string ListenerArn { get; set; }
        
        public string[] RuleArns { get; set; }
    }
}
