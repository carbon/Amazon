﻿using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class StopInstancesRequest : IEc2Request
{
    public StopInstancesRequest(params string[] instanceIds)
    {
        Ensure.NotEmpty(instanceIds);

        InstanceIds = instanceIds;
    }

    public bool? DryRun { get; init; }

    public bool? Force { get; init; }

    [DataMember(Name = "InstanceId")]
    public string[] InstanceIds { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        return Ec2RequestSerializer.ToParams("StopInstances", this);
    }
}