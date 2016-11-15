using System;

using Carbon.Data;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class GetItemRequest
    {
        public GetItemRequest(string tableName, RecordKey key)
        {
            #region Preconditions

            if (tableName == null) throw new ArgumentNullException(nameof(tableName));

            #endregion

            TableName = tableName;
            Key = key;
        }

        // [Required]
        public string TableName { get; }

       //  [Required]
        public RecordKey Key { get; }

        public bool ConsistentRead { get; set; }

        public string[] AttributesToGet { get; set; }

        public bool ReturnConsumedCapacity { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject {
                { "TableName", TableName },
                { "Key",       Key.ToJson() },
            };

            if (ConsistentRead)             json.Add("ConsistentRead", ConsistentRead);
            if (ReturnConsumedCapacity)     json.Add("ReturnConsumedCapacity", "TOTAL");
            if (AttributesToGet != null)    json.Add("AttributesToGet", JsonArray.Create(AttributesToGet));

            return json;
        }
    }
}

/*
{
    "AttributesToGet": [
        "string"
    ],
    "ConsistentRead": "boolean",
    "Key": {
        "string" :
            {
                "B": "blob",
                "BS": [
                    "blob"
                ],
                "N": "string",
                "NS": [
                    "string"
                ],
                "S": "string",
                "SS": [
                    "string"
                ]
            }
    },
    "ReturnConsumedCapacity": "string",
    "TableName": "string"
}
*/
