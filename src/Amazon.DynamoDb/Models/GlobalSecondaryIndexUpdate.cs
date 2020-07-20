namespace Amazon.DynamoDb
{
    public sealed class GlobalSecondaryIndexUpdate
    {
        public CreateGlobalSecondaryIndexAction? Create { get; set; }

        public DeleteGlobalSecondaryIndexAction? Delete { get; set; }

        public UpdateGlobalSecondaryIndexAction? Update { get; set; }
    }
}