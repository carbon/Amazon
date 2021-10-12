namespace Amazon.DynamoDb
{
    public sealed class GetItemRequest
    {
        public GetItemRequest(string tableName, IEnumerable<KeyValuePair<string, object>> key)
            : this(tableName, key.ToDictionary()) { }

        public GetItemRequest(string tableName, IReadOnlyDictionary<string, DbValue> key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public string TableName { get; }

        public IReadOnlyDictionary<string, DbValue> Key { get; }

        public bool? ConsistentRead { get; set; }

        public string[]? AttributesToGet { get; set; }

        public ReturnConsumedCapacity? ReturnConsumedCapacity { get; set; }
    }
}