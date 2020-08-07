namespace Amazon.DynamoDb.Models
{
    public sealed class GlobalSecondaryIndexUpdate
    {
        public CreateGlobalSecondaryIndexAction? Create { get; set; }

        public DeleteGlobalSecondaryIndexAction? Delete { get; set; }

        public UpdateGlobalSecondaryIndexAction? Update { get; set; }
    }
}