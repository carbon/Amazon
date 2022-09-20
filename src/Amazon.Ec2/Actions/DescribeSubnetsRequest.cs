﻿namespace Amazon.Ec2;

public sealed class DescribeSubnetsRequest : DescribeRequest, IEc2Request
{
    public DescribeSubnetsRequest(params string[] subnetIds)
    {
        SubnetIds = subnetIds;
    }

    public string[] SubnetIds { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        var parameters = GetParameters("DescribeSubnets");

        AddIds(parameters, "SubnetId", SubnetIds);

        return parameters;
    }
}
