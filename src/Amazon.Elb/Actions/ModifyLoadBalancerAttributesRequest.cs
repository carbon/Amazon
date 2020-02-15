#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class ModifyLoadBalancerAttributesRequest : IElbRequest
    {
        public string Action => "ModifyLoadBalancerAttributes";

        [Required]
        public LoadBalancerAttribute[] Attributes { get; set; }
        
        [Required]
        public string LoadBalancerArn { get; set; }
    }
}