using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class DescribeLoadBalancerAttributesRequest : IElbRequest
    {
        public string Action => "DescribeLoadBalancerAttributes";

        [Required]
        public string LoadBalancerArn { get; set; }
    }
}
