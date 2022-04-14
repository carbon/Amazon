namespace Amazon.DynamoDb.Models;

public sealed class DeleteGlobalSecondaryIndexAction
{
    public DeleteGlobalSecondaryIndexAction(string indexName!!)
    {
        IndexName = indexName;
    }

    public string IndexName { get; }
}