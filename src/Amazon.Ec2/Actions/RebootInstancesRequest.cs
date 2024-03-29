﻿using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class RebootInstancesRequest : IEc2Request
{
    public RebootInstancesRequest(params string[] instanceIds)
    {
        Ensure.NotEmpty(instanceIds);

        InstanceIds = instanceIds;
    }

    public bool? DryRun { get; init; }

    [DataMember(Name = "InstanceId")]
    public string[] InstanceIds { get; }

     List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        return Ec2RequestSerializer.ToParams("RebootInstances", this);
    }
}