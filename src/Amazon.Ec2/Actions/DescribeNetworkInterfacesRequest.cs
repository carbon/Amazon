﻿namespace Amazon.Ec2;

public sealed class DescribeNetworkInterfacesRequest : DescribeRequest, IEc2Request
{
    public DescribeNetworkInterfacesRequest(params string[] networkInterfaceIds)
    {
        ArgumentNullException.ThrowIfNull(networkInterfaceIds);

        NetworkInterfaceIds = networkInterfaceIds;
    }

    public string[] NetworkInterfaceIds { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        var parameters = GetParameters("DescribeNetworkInterfaces");

        AddIds(parameters, "NetworkInterfaceId", NetworkInterfaceIds);

        return parameters;
    }
}
