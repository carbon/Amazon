using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class DeleteLoadBalancerRequest : IElbRequest
    {
        public string Action => "DeleteLoadBalancer";
        
        [Required]
        public string LoadBalancerArn { get; set; }
    }
}