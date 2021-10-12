﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class StopInstancesRequest : IEc2Request
{
    public StopInstancesRequest(params string[] instanceIds)
    {
        InstanceIds = instanceIds ?? throw new ArgumentNullException(nameof(instanceIds));
    }

    public bool? DryRun { get; init; }

    public bool? Force { get; init; }

    [DataMember(Name = "InstanceId")]
    public string[] InstanceIds { get; }

    public Dictionary<string, string> ToParams()
    {
        return Ec2RequestHelper.ToParams("StopInstances", this);
    }
}