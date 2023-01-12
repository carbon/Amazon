using System.Diagnostics.CodeAnalysis;

namespace Amazon.Ssm;

public sealed class DeregisterManagedInstanceRequest : ISsmRequest
{
    public DeregisterManagedInstanceRequest() { }

    [SetsRequiredMembers]
    public DeregisterManagedInstanceRequest(string instanceId)
    {
        ArgumentException.ThrowIfNullOrEmpty(instanceId);

        InstanceId = instanceId;
    }

    public required string InstanceId { get; init; }
}