namespace Amazon.DynamoDb
{
    public sealed class ListTablesRequest
    {
        public string? ExclusiveStartTableName { get; init; }

        public int? Limit { get; init; }
    }
}