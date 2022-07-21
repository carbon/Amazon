﻿using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class TerminateInstancesRequest : IEc2Request
{
    public TerminateInstancesRequest(params string[] instanceIds)
    {
        ArgumentNullException.ThrowIfNull(instanceIds);

        InstanceIds = instanceIds;
    }

    [DataMember]
    public bool? DryRun { get; init; }

    [DataMember(Name = "InstanceId")]
    public string[] InstanceIds { get; }

    public Dictionary<string, string> ToParams()
    {
        return Ec2RequestHelper.ToParams("TerminateInstances", this);
    }
}