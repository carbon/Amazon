using System.Collections.Generic;

namespace Amazon.DynamoDb
{
    public sealed class TransactWriteItemsResult
    {
        public TransactionConsumedCapacity[]? ConsumedCapacity { get; init; }

        public Dictionary<string, ReturnItemCollectionMetrics>? ItemCollectionMetrics { get; init; }
    }
}
