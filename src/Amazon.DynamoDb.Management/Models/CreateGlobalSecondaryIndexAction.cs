namespace Amazon.DynamoDb.Models;

public sealed class CreateGlobalSecondaryIndexAction
{
    public CreateGlobalSecondaryIndexAction(
        string indexName!!,
        KeySchemaElement[] keySchema!!,
        Projection projection!!)
    {
        IndexName = indexName;
        KeySchema = keySchema;
        Projection = projection;
    }

    public string IndexName { get; }

    public KeySchemaElement[] KeySchema { get; }

    public Projection Projection { get; }

    public ProvisionedThroughput? ProvisionedThroughput { get; set; }
}
