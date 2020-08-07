namespace Amazon.DynamoDb
{
    public sealed class ListTablesRequest
    {
        public string? ExclusiveStartTableName { get; set; }

        public int? Limit { get; set; }
    }
}