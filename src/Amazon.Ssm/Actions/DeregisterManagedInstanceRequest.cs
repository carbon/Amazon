#nullable disable

using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class DeregisterManagedInstanceRequest : ISsmRequest
{
    public DeregisterManagedInstanceRequest() { }

    public DeregisterManagedInstanceRequest(string instanceId)
    {
        ArgumentNullException.ThrowIfNull(instanceId);

        InstanceId = instanceId;
    }

    [Required]
    public string InstanceId { get; init; }
}