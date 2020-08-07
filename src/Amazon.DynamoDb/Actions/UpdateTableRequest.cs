using System;

using Amazon.DynamoDb.Models;

namespace Amazon.DynamoDb
{
    public sealed class UpdateTableRequest
    {
        public UpdateTableRequest(string tableName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public string TableName { get; }

        public AttributeDefinition[]? AttributeDefinitions { get; set; }

        public BillingMode? BillingMode { get; set; }

        public GlobalSecondaryIndexUpdate[]? GlobalSecondaryIndexUpdates { get; set; }

        public ProvisionedThroughput? ProvisionedThroughput { get; set; }

        public ReplicationGroupUpdate[]? ReplicaUpdates { get; set; }

        public SSESpecification? SSESpecification { get; set; }

        public StreamSpecification? StreamSpecification { get; set; }
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
