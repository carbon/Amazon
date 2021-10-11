using System;

namespace Amazon.DynamoDb.Models;

public sealed class DeleteGlobalSecondaryIndexAction
{
    public DeleteGlobalSecondaryIndexAction(string indexName)
    {
        ArgumentNullException.ThrowIfNull(indexName);

        IndexName = indexName;
    }

    public string IndexName { get; }
}