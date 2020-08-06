using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class TransactWriteItemsResult
    {
        public TransactionConsumedCapacity[]? ConsumedCapacity { get; set; }
        public Dictionary<string, ReturnItemCollectionMetrics>? ItemCollectionMetrics { get; set; }
    }
}
