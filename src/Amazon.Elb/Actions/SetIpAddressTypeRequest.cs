namespace Amazon.Elb;

public sealed class SetIpAddressTypeRequest : IElbRequest
{
    public string Action => "SetIpAddressType";

    public SetIpAddressTypeRequest(string ipAddressType, string loadBalancerArn)
    {
        ArgumentNullException.ThrowIfNull(ipAddressType);
        ArgumentNullException.ThrowIfNull(loadBalancerArn);

        IpAddressType = ipAddressType;
        LoadBalancerArn = loadBalancerArn;
    }

    public string IpAddressType { get; }

    public string LoadBalancerArn { get; }
}