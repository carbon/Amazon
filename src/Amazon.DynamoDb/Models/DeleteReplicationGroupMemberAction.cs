namespace Amazon.DynamoDb
{
    public sealed class DeleteReplicationGroupMemberAction
    {
        public DeleteReplicationGroupMemberAction(string regionName)
        {
            RegionName = regionName;
        }

        public string RegionName { get; }
    }
}
