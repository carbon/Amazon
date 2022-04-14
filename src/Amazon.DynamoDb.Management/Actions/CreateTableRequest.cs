using System.Collections.Generic;
using System.Text.Json.Serialization;

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb;

public sealed class CreateTableRequest
{
    public CreateTableRequest(
        string tableName!!,
        AttributeDefinition[] attributeDefinitions!!,
        IEnumerable<KeySchemaElement> keySchema!!)
    {
        TableName = tableName;
        AttributeDefinitions = attributeDefinitions;
        KeySchema = keySchema;
    }

    public string TableName { get; }

    public BillingMode BillingMode { get; set; }

    public AttributeDefinition[] AttributeDefinitions { get; }

    public IEnumerable<KeySchemaElement> KeySchema { get; }

    public IEnumerable<LocalSecondaryIndex>? LocalSecondaryIndexes { get; set; }

    public IEnumerable<GlobalSecondaryIndex>? GlobalSecondaryIndexes { get; set; }

    public ProvisionedThroughput? ProvisionedThroughput { get; set; }

    [JsonPropertyName("SSESpecification")]
    public SseSpecification? SseSpecification { get; set; }

    public StreamSpecification? StreamSpecification { get; set; }

    public IList<Tag>? Tags { get; set; }
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
