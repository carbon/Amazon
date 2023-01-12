namespace Amazon.Ssm;

public sealed class UpdateManagedInstanceRoleRequest : ISsmRequest
{
    public required string IamRole { get; init; }

    public required string InstanceId { get; init; }
}
