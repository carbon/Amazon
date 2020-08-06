using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Amazon.DynamoDb.JsonConverters;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(BatchGetItemRequestConverter))]
    public sealed class BatchGetItemRequest
    {
        public BatchGetItemRequest(params TableKeys[] sets)
        {
            Sets = sets ?? throw new ArgumentNullException(nameof(sets));
        }

        public TableKeys[] Sets { get; }
    }

    public sealed class TableKeys
    {
        public TableKeys(string tableName, params Dictionary<string, DbValue>[] keys)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(TableName));
            Keys = keys;
        }

        public string TableName { get; }

        public Dictionary<string, DbValue>[] Keys { get; }

        public string[]? AttributesToGet { get; set; }

        public bool ConsistentRead { get; set; }

        /* 
		{
		 "AttributesToGet": [ "string" ],
         "ConsistentRead": "boolean",
         "Keys": [
                   { "Name":{"S":"Amazon DynamoDB"} },
                   { "Name":{"S":"Amazon RDS"} },
                   { "Name":{"S":"Amazon Redshift"} }
                 ]
		}
		*/

    }
}

/*
{ 
  "RequestItems":  {
	  "tableName" : {
			"AttributesToGet": [ "string" ],
			"ConsistentRead": "boolean",
			"Keys": [
					  { "Name":{"S":"Amazon DynamoDB"} },
					  { "Name":{"S":"Amazon RDS"} },
					  { "Name":{"S":"Amazon Redshift"} }
					]
		}
	  }
  },
  "ReturnConsumedCapacity": "string"
}
*/
