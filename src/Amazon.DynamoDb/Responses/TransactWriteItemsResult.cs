using System.Collections.Generic;

namespace Amazon.DynamoDb
{
    public sealed class TransactWriteItemsResult
    {
        public TransactionConsumedCapacity[]? ConsumedCapacity { get; set; }

        public Dictionary<string, ReturnItemCollectionMetrics>? ItemCollectionMetrics { get; set; }
    }
}
