#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public sealed class ModifyLoadBalancerAttributesRequest : IElbRequest
    {
        public string Action => "ModifyLoadBalancerAttributes";

        [Required]
        public LoadBalancerAttribute[] Attributes { get; init; }
        
        [Required]
        public string LoadBalancerArn { get; init; }
    }
}