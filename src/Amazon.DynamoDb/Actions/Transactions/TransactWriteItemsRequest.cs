using System;

namespace Amazon.DynamoDb.Transactions
{
    public sealed class TransactWriteItemsRequest
    {
        public TransactWriteItemsRequest(params TransactWriteItem[] transactItems)
        {
            TransactItems = transactItems ?? throw new ArgumentNullException(nameof(transactItems));
        }

        public TransactWriteItem[] TransactItems { get; set; }

        public string? ClientRequestToken { get; set; }

        public ReturnConsumedCapacity? ReturnConsumedCapacity { get; set; }

        public ReturnItemCollectionMetrics? ReturnItemCollectionMetrics { get; set; }
    }
}