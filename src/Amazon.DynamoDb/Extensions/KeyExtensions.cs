#nullable enable

using System.Collections.Generic;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    internal static class KeyExtensions
    {
        internal static JsonObject ToJson(this IEnumerable<KeyValuePair<string, object>> key)
        {
            var json = new JsonObject();

            foreach (var item in key)
            {
                json.Add(item.Key, new DbValue(item.Value).ToJson());
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
