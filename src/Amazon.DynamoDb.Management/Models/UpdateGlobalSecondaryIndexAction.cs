using System;

namespace Amazon.DynamoDb.Models;

public sealed class UpdateGlobalSecondaryIndexAction
{
    public UpdateGlobalSecondaryIndexAction(
        string indexName,
        ProvisionedThroughput provisionedThroughput)
    {
        ArgumentNullException.ThrowIfNull(indexName);
        ArgumentNullException.ThrowIfNull(provisionedThroughput);

        IndexName = indexName;
        ProvisionedThroughput = provisionedThroughput;
    }

    public string IndexName { get; }

    public ProvisionedThroughput ProvisionedThroughput { get; }
}