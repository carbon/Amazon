using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class GetItemRequest
    {
        public GetItemRequest(string tableName, IEnumerable<KeyValuePair<string, object>> key)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        [DataMember(Name = "TableName", IsRequired = true)]
        public string TableName { get; }

        [DataMember(Name = "Key", IsRequired = true)]
        public IEnumerable<KeyValuePair<string, object>> Key { get; }

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
