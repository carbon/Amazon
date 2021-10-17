namespace Amazon.DynamoDb.Transactions;

public sealed class TransactWriteItemsRequest
{
    public TransactWriteItemsRequest(params TransactWriteItem[] transactItems)
    {
        ArgumentNullException.ThrowIfNull(transactItems);

        TransactItems = transactItems;
    }

    public TransactWriteItem[] TransactItems { get; init; }

    public string? ClientRequestToken { get; init; }

    public ReturnConsumedCapacity? ReturnConsumedCapacity { get; init; }

    public ReturnItemCollectionMetrics? ReturnItemCollectionMetrics { get; init; }
}
