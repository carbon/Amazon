using System;
using System.Collections.Generic;

namespace Amazon.DynamoDb.Transactions
{
    public sealed class TransactGetItemRequest
    {
        public TransactGetItem[] TransactItems { get; init; }

        public ReturnConsumedCapacity? ReturnConsumedCapacity { get; init; }

        public TransactGetItemRequest(TransactGetItem[] transactItems)
        {
            TransactItems = transactItems ?? throw new ArgumentNullException(nameof(transactItems));
        }
    }

    public sealed class TransactGetItem
    {
        public TransactGetItem(Get get)
        {
            Get = get ?? throw new ArgumentNullException(nameof(get));
        }

        public Get Get { get; init; }
    }

    public sealed class Get
    {
        public Get(string tableName, IReadOnlyDictionary<string, DbValue> key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public string TableName { get; init; }

        public IReadOnlyDictionary<string, DbValue> Key { get; init; }

        public Dictionary<string, string>? ExpressionAttributeNames { get; init; }

        public string? ProjectionExpression { get; init; }
    }
}