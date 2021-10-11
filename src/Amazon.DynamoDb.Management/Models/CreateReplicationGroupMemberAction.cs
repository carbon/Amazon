namespace Amazon.DynamoDb.Models;

public sealed class CreateReplicationGroupMemberAction : ReplicationGroupMemberAction
{
    public CreateReplicationGroupMemberAction(string regionName)
        : base(regionName)
    {
    }
}
