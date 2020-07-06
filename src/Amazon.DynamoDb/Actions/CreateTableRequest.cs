using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.Models;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public sealed class CreateTableRequest
    {
        public CreateTableRequest(string tableName, AttributeDefinition[] attributeDefinitions, IEnumerable<KeySchemaElement> keySchema)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            AttributeDefinitions = attributeDefinitions ?? throw new ArgumentNullException(nameof(attributeDefinitions));
            KeySchema = keySchema ?? throw new ArgumentNullException(nameof(keySchema));
        }

        public string TableName { get; }

        public BillingMode BillingMode { get; set; }

        public AttributeDefinition[] AttributeDefinitions { get; }

        public IEnumerable<KeySchemaElement> KeySchema { get; }

        public IEnumerable<LocalSecondaryIndex>? LocalSecondaryIndexes { get; set; }
        public IEnumerable<GlobalSecondaryIndex>? GlobalSecondaryIndexes { get; set; }

        public ProvisionedThroughput? ProvisionedThroughput { get; set; }

        public SSESpecification? SSESpecification { get; set; }

        public StreamSpecification? StreamSpecification { get; set; }

        public IEnumerable<KeyValuePair<string, string>>? Tags { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName", TableName },
                { "BillingMode", BillingMode.ToQuickString() },
                { "KeySchema", KeySchema.ToJson() },
                { "AttributeDefinitions", AttributeDefinitions.ToJson() },
            };

            if (LocalSecondaryIndexes != null)   json.Add("LocalSecondaryIndexes", LocalSecondaryIndexes.ToJson());
            if (GlobalSecondaryIndexes != null)  json.Add("GlobalSecondaryIndexes", GlobalSecondaryIndexes.ToJson());
            if (ProvisionedThroughput != null)   json.Add("ProvisionedThroughput", ProvisionedThroughput.ToJson());
            if (SSESpecification != null)        json.Add("SSESpecification", SSESpecification.ToJson());
            if (StreamSpecification != null)     json.Add("StreamSpecification", StreamSpecification.ToJson());
            if (Tags != null)                    json.Add("Tags", Tags.ToJson());

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
