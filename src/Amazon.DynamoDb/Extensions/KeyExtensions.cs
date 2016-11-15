using Carbon.Data;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    internal static class KeyExtensions
    {
        internal static JsonObject ToJson(this RecordKey key)
        {
            var json = new JsonObject();

            foreach (var attribute in key.Attributes)
            {
                json.Add(attribute.Key, new DbValue(attribute.Value).ToJson());
            }

            return json;
        }
    }
}


/*
{
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
*/
