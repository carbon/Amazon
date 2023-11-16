namespace Amazon.DynamoDb.Transactions;

public sealed class TransactWriteItemsRequest(params TransactWriteItem[] transactItems)
{
    public required TransactWriteItem[] TransactItems { get; init; } = transactItems;

    public string? ClientRequestToken { get; init; }

    public ReturnConsumedCapacity? ReturnConsumedCapacity { get; init; }

    public ReturnItemCollectionMetrics? ReturnItemCollectionMetrics { get; init; }
}