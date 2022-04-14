﻿namespace Amazon.Elb;

public sealed class DeleteLoadBalancerRequest : IElbRequest
{
    public DeleteLoadBalancerRequest(string loadBalancerArn!!)
    {
        LoadBalancerArn = loadBalancerArn;
    }

    public string Action => "DeleteLoadBalancer";

    public string LoadBalancerArn { get; }
}