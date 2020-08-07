namespace Amazon.DynamoDb.Models
{
    public sealed class GlobalSecondaryIndex : LocalSecondaryIndex
    {
        public GlobalSecondaryIndex(string indexName, KeySchemaElement[] keySchema, Projection projection)
            : base(indexName, keySchema, projection)
        {
            
        }

        public ProvisionedThroughput? ProvisionedThroughput { get; set; }
    }
}