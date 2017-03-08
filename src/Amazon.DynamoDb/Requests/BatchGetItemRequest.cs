using System;
using System.Collections.Generic;
using System.Linq;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class BatchGetItemRequest
    {
        private readonly TableKeys[] sets;

        public BatchGetItemRequest(params TableKeys[] sets)
        {
            this.sets = sets ?? throw new ArgumentNullException(nameof(sets));
        }

        // [Required]
        public TableKeys[] Sets => sets;

        public JsonObject ToJson()
        {
            var o = new JsonObject();

            foreach (var set in sets)
            {
                o.Add(set.TableName, set.ToJson());
            }

            return new JsonObject {
                { "RequestItems", o }
            };
        }
    }

    public class TableKeys
    {
        public TableKeys(string tableName, params IEnumerable<KeyValuePair<string, object>>[] keys)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(TableName));
            Keys = keys;
        }

        public string TableName { get; }

        public IEnumerable<KeyValuePair<string, object>>[] Keys { get; }

        public string[] AttributesToGet { get; set; }

        public bool ConsistentRead { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "Keys", new XNodeArray(Keys.Select(k => k.ToJson()).ToArray()) }
            };

            if (AttributesToGet != null) json.Add("AttributesToGet", JsonArray.Create(AttributesToGet));
            if (ConsistentRead) json.Add("ConsistentRead", ConsistentRead);

            return json;
        }


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
