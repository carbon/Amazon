namespace Amazon.DynamoDb.Transactions
{
    public sealed class TransactWriteItem
    {
        public ConditionCheck? ConditionCheck { get; set; }

        public DeleteItem? Delete { get; set; }

        public PutItem? Put { get; set; }

        public UpdateItem? Update { get; set; }
    }
}