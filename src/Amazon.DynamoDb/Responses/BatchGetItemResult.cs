using System.Collections.Generic;
using System.Collections.ObjectModel;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class BatchGetItemResult // : IConsumedResources
    {
        // public ConsumedCapacity[] ConsumedCapacity { get; set; }

        public List<TableItemCollection> Responses { get; } = new List<TableItemCollection>();

        public List<TableKeys> UnprocessedKeys { get; } = new List<TableKeys>();

        public static BatchGetItemResult FromJson(JsonObject json)
        {
            var result = new BatchGetItemResult();

            var items = new List<AttributeCollection>();

            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode)) // Array
            {
                foreach (var item in (JsonArray)consumedCapacityNode)
                {
                    var unit = item.As<ConsumedCapacity>();

                    // TODO
                }
            }

            if (json.TryGetValue("Responses", out var responsesNode))
            {
                foreach (var tableEl in (JsonObject)responsesNode)
                {
                    var table = new TableItemCollection(tableEl.Key);

                    foreach (var item in (JsonArray)tableEl.Value)
                    {
                        table.Add(AttributeCollection.FromJson((JsonObject)item));
                    }

                    result.Responses.Add(table);
                }
            }

            /*
			if (json.TryGetValue("UnprocessedKeys", out var unprocessedKeysNode))
			{
				foreach (var tableEl in (JsonObject)unprocessedKeysNode)
				{
					var tableName = tableEl.Key;

					foreach (var keyEl in (XArray) tableEl.Value["Keys"])
					{
						var attributes = new KeyValuePair<string, object>();

						foreach (var prop in (JsonObject)keyEl)
						{
							var name = prop.Key;
							var value = DbValue.FromJson((JsonObject)prop.Value);
						}

						var key = new Key(attributes);
					}
				}
			}
			*/

            return result;
        }
    }

    public sealed class TableItemCollection : Collection<AttributeCollection>
    {
        public TableItemCollection(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}

/*
{
    "ConsumedCapacity": [
        {
            "CapacityUnits": "number",
            "TableName": "string"
        }
    ],
    "Responses": 
        {
            "tableName" : [
                    
                {
                    "string" : {
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
                }
            ]
        },
    "UnprocessedKeys": 
        {
            "string" :
                {
                    "AttributesToGet": [
                        "string"
                    ],
                    "ConsistentRead": "boolean",
                    "Keys": [
                        
                            { "string" : { 
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
                            }
                    ]
                }
        }
}
*/
