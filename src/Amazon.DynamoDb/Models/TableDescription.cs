using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
{
    public class TableDescription : IConvertibleFromJson
    {
        public TableDescription() { }

        public ArchivalSummary? ArchivalSummary { get; set; }
        public AttributeDefinitions? AttributeDefinitions { get; set; }
        public BillingModeSummary? BillingModeSummary { get; set; }
        public DateTimeOffset CreationDateTime { get; set; }
        public GlobalSecondaryIndexDescription[]? GlobalSecondaryIndexes { get; set; }
        public string? GlobalTableVersion { get; set; }
        public long ItemCount { get; set; }
        public KeySchemaElement[]? KeySchema { get; set; }
        public string? LatestStreamArn { get; set; }
        public string? LatestStreamLabel { get; set; }
        public LocalSecondaryIndexDescription[]? LocalSecondaryIndexes { get; set; }
        public ProvisionedThroughput? ProvisionedThroughput { get; set; }
        public ReplicaDescription[]? Replicas { get; set; }
        public RestoreSummary[]? RestoreSummary { get; set;}
        public SSEDescription? SSEDescription { get; set; }
        public StreamSpecification? StreamSpecification { get; set; }
        public string? TableArn { get; set; }
        public string? TableId { get; set; }
        public string? TableName { get; set; }
        public long TableSizeBytes { get; set; }
        public TableStatus TableStatus { get; set; }

        public void FillField(JsonProperty p)
        {
            if (p.NameEquals("ArchivalSummary")) ArchivalSummary = p.Value.GetObject<ArchivalSummary>();
            else if (p.NameEquals("AttributeDefinitions")) AttributeDefinitions = AttributeDefinitions.FromJsonElement(p.Value);
            else if (p.NameEquals("BillingModeSummary")) BillingModeSummary = p.Value.GetObject<BillingModeSummary>();
            else if (p.NameEquals("CreationDateTime")) CreationDateTime = p.Value.GetDynamoDateTimeOffset();
            else if (p.NameEquals("GlobalSecondaryIndexes")) GlobalSecondaryIndexes = p.Value.GetObjectArray<GlobalSecondaryIndexDescription>();
            else if (p.NameEquals("GlobalTableVersion")) GlobalTableVersion = p.Value.GetString();
            else if (p.NameEquals("ItemCount")) ItemCount = p.Value.GetInt64();
            else if (p.NameEquals("KeySchema")) KeySchema = p.Value.GetObjectArray<KeySchemaElement>();
            else if (p.NameEquals("LatestStreamArn")) LatestStreamArn = p.Value.GetString();
            else if (p.NameEquals("LatestStreamLabel")) LatestStreamLabel = p.Value.GetString();
            else if (p.NameEquals("LocalSecondaryIndexes")) LocalSecondaryIndexes = p.Value.GetObjectArray<LocalSecondaryIndexDescription>();
            else if (p.NameEquals("ProvisionedThroughput")) ProvisionedThroughput = p.Value.GetObject<ProvisionedThroughput>();
            else if (p.NameEquals("Replicas")) Replicas = p.Value.GetObjectArray<ReplicaDescription>();
            else if (p.NameEquals("RestoreSummary")) RestoreSummary = p.Value.GetObjectArray<RestoreSummary>();
            else if (p.NameEquals("SSEDescription")) SSEDescription = p.Value.GetObject<SSEDescription>();
            else if (p.NameEquals("StreamSpecification")) StreamSpecification = p.Value.GetObject<StreamSpecification>();
            else if (p.NameEquals("TableArn")) TableArn = p.Value.GetString();
            else if (p.NameEquals("TableId")) TableId = p.Value.GetString();
            else if (p.NameEquals("TableName")) TableName = p.Value.GetString();
            else if (p.NameEquals("TableSizeBytes")) TableSizeBytes = p.Value.GetInt64();
            else if (p.NameEquals("TableStatus")) TableStatus = p.Value.GetEnum<TableStatus>();
        }

    }
}
