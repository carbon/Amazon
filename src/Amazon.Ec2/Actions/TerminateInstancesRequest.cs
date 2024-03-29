﻿using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class TerminateInstancesRequest : IEc2Request
{
    public TerminateInstancesRequest(params string[] instanceIds)
    {
        Ensure.NotEmpty(instanceIds);

        InstanceIds = instanceIds;
    }

    [DataMember]
    public bool? DryRun { get; init; }

    [DataMember(Name = "InstanceId")]
    public string[] InstanceIds { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        return Ec2RequestSerializer.ToParams("TerminateInstances", this);
    }
}