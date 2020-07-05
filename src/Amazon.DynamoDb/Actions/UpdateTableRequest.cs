using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.Models;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class UpdateTableRequest
    {
        public UpdateTableRequest(string tableName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public string TableName { get; }

        public AttributeDefinitions? AttributeDefinitions { get; set; } 
        public BillingMode? BillingMode { get; set; }
        public GlobalSecondaryIndexUpdate[]? GlobalSecondaryIndexes { get; set; }

        public ProvisionedThroughput? ProvisionedThroughput { get; set; }
        public ReplicationGroupUpdate[]? ReplicaUpdates { get; set; }
        public SSESpecification? SSESpecification { get; set; }

        public StreamSpecification? StreamSpecification { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName", TableName },
            };

            if (AttributeDefinitions != null)    json.Add("AttributeDefinitions", AttributeDefinitions.ToJson());
            if (BillingMode.HasValue)            json.Add("BillingMode", BillingMode.Value.ToQuickString());
            if (GlobalSecondaryIndexes != null)  json.Add("GlobalSecondaryIndexes", GlobalSecondaryIndexes.ToJson());
            if (ProvisionedThroughput != null)   json.Add("ProvisionedThroughput", ProvisionedThroughput.ToJson());
            if (ReplicaUpdates != null)          json.Add("ReplicaUpdates", ReplicaUpdates.ToJson());
            if (SSESpecification != null)        json.Add("SSESpecification", SSESpecification.ToJson());
            if (StreamSpecification != null)     json.Add("StreamSpecification", StreamSpecification.ToJson());

            return json;
        }
    }
}

/*
{
   "AttributeDefinitions": [ 
      { 
         "AttributeName": "string",
         "AttributeType": "string"
      }
   ],
   "BillingMode": "string",
   "GlobalSecondaryIndexes": [ 
      { 
         "IndexName": "string",
         "KeySchema": [ 
            { 
               "AttributeName": "string",
               "KeyType": "string"
            }
         ],
         "Projection": { 
            "NonKeyAttributes": [ "string" ],
            "ProjectionType": "string"
         },
         "ProvisionedThroughput": { 
            "ReadCapacityUnits": number,
            "WriteCapacityUnits": number
         }
      }
   ],
   "KeySchema": [ 
      { 
         "AttributeName": "string",
         "KeyType": "string"
      }
   ],
   "LocalSecondaryIndexes": [ 
      { 
         "IndexName": "string",
         "KeySchema": [ 
            { 
               "AttributeName": "string",
               "KeyType": "string"
            }
         ],
         "Projection": { 
            "NonKeyAttributes": [ "string" ],
            "ProjectionType": "string"
         }
      }
   ],
   "ProvisionedThroughput": { 
      "ReadCapacityUnits": number,
      "WriteCapacityUnits": number
   },
   "SSESpecification": { 
      "Enabled": boolean,
      "KMSMasterKeyId": "string",
      "SSEType": "string"
   },
   "StreamSpecification": { 
      "StreamEnabled": boolean,
      "StreamViewType": "string"
   },
   "TableName": "string",
   "Tags": [ 
      { 
         "Key": "string",
         "Value": "string"
      }
   ]
}
*/
