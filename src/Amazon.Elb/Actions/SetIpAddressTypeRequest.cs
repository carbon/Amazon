using System;

namespace Amazon.Elb
{
    public sealed class SetIpAddressTypeRequest : IElbRequest
    {
        public string Action => "SetIpAddressType";

        public SetIpAddressTypeRequest(string ipAddressType, string loadBalancerArn)
        {
            IpAddressType = ipAddressType ?? throw new ArgumentNullException(nameof(ipAddressType));
            LoadBalancerArn = loadBalancerArn ?? throw new ArgumentNullException(nameof(ipAddressType));
        }

        public string IpAddressType { get; }
        
        public string LoadBalancerArn { get; }
    }
}