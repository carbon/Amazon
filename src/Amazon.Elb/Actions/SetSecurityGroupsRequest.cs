using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class SetSecurityGroupsRequest : IElbRequest
    {
        public string Action => "SetSecurityGroups";

        [Required]
        public string LoadBalancerArn { get; set; }

        [Required]
        public string[] SecurityGroups { get; set; }
    }
}