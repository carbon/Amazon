using System;
using System.Collections.Generic;

namespace Amazon.DynamoDb
{
    public sealed class TransactGetItemRequest
    {
        public TransactGetItem[] TransactItems { get; set; }

        public ReturnConsumedCapacity? ReturnConsumedCapacity { get; set; }

        public TransactGetItemRequest(TransactGetItem[] transactItems)
        {
            TransactItems = transactItems ?? throw new ArgumentNullException(nameof(transactItems));
        }
    }

    public sealed class TransactGetItem
    {
        public Get Get { get; set; }

        public TransactGetItem(Get get)
        {
            Get = get ?? throw new ArgumentNullException(nameof(get));
        }
    }

    public sealed class Get
    {
        public Get(string tableName, IReadOnlyDictionary<string, DbValue> key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public string TableName { get; set; }

        public IReadOnlyDictionary<string, DbValue> Key { get; set; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; set; }

        public string? ProjectionExpression { get; set; }
    }
}