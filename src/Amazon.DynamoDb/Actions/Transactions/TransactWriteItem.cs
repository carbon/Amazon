namespace Amazon.DynamoDb.Transactions;

public sealed class TransactWriteItem
{
    public ConditionCheck? ConditionCheck { get; init; }

    public DeleteItem? Delete { get; init; }

    public PutItem? Put { get; init; }

    public UpdateItem? Update { get; init; }
}