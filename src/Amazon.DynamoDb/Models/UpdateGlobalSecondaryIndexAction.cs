using System;

namespace Amazon.DynamoDb
{
    public sealed class UpdateGlobalSecondaryIndexAction
    {
        public UpdateGlobalSecondaryIndexAction(string indexName, ProvisionedThroughput provisionedThroughput)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
            ProvisionedThroughput = provisionedThroughput ?? throw new ArgumentNullException(nameof(provisionedThroughput));
        }

        public string IndexName { get; }

        public ProvisionedThroughput ProvisionedThroughput { get; set; }
    }
}
