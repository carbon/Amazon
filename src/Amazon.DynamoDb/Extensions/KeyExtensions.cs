using System.Collections.Generic;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    internal static class KeyExtensions
    {
        public static JsonObject ToJson(this IEnumerable<KeyValuePair<string, object>> key)
        {
            var json = new JsonObject();

            foreach (var item in key)
            {
                json.Add(item.Key, new DbValue(item.Value).ToJson());
            }

            return json;
        }


        public static JsonNodeList ToNodeList(this IEnumerable<KeyValuePair<string, object>>[] values)
        {
            var items = new JsonObject[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                IEnumerable<KeyValuePair<string, object>> item = values[i];

                items[i] = item.ToJson();
            }

            return new JsonNodeList(items);
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
