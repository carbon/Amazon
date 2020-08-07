using System;
using System.Collections.Generic;

namespace Amazon.DynamoDb
{
    public sealed class CreateTableRequest
    {
        public CreateTableRequest(
            string tableName, 
            AttributeDefinition[] attributeDefinitions,
            IEnumerable<KeySchemaElement> keySchema)
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

        public IEnumerable<Tag>? Tags { get; set; }
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
