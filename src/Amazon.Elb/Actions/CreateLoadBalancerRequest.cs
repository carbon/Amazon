using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class CreateLoadBalancerRequest : IElbRequest
    {
        public string Action => "CreateLoadBalancer";

        public string IpAddressType { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        public string Scheme { get; set; }

        public string[] SecurityGroups { get; set; }

        // Must specifiy at least 2 subnets
        [Required]
        public string[] Subnets { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
