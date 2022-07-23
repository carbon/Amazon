using System;
using System.Text.Json.Serialization;

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb;

public sealed class UpdateTableRequest
{
    public UpdateTableRequest(string tableName)
    {
        ArgumentNullException.ThrowIfNull(tableName);

        TableName = tableName;
    }

    public string TableName { get; }

    public AttributeDefinition[]? AttributeDefinitions { get; set; }

    public BillingMode? BillingMode { get; set; }

    public GlobalSecondaryIndexUpdate[]? GlobalSecondaryIndexUpdates { get; set; }

    public ProvisionedThroughput? ProvisionedThroughput { get; set; }

    public ReplicationGroupUpdate[]? ReplicaUpdates { get; set; }

    [JsonPropertyName("SSESpecification")]
    public SseSpecification? SseSpecification { get; set; }

    public StreamSpecification? StreamSpecification { get; set; }
}
