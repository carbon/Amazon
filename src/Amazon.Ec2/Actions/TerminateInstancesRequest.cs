using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class TerminateInstancesRequest : IEc2Request
{
    public TerminateInstancesRequest(params string[] instanceIds)
    {
        InstanceIds = instanceIds ?? throw new ArgumentNullException(nameof(instanceIds));
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