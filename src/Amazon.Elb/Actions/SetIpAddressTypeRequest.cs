using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class SetIpAddressTypeRequest : IElbRequest
    {
        public string Action => "SetIpAddressType";

        [Required]
        public string IpAddressType { get; set; }
        
        [Required]
        public string LoadBalancerArn { get; set; }
    }
}