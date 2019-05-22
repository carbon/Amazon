#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class BatchGetItemResult // : IConsumedResources
    {
        public BatchGetItemResult(TableItemCollection[] responses)
        {
            this.Responses = responses;
        }

        // public ConsumedCapacity[] ConsumedCapacity { get; set; }

        public TableItemCollection[] Responses { get; }

        public IReadOnlyList<TableKeys> UnprocessedKeys => Array.Empty<TableKeys>();

        public static BatchGetItemResult FromJson(JsonObject json)
        {
            // var items = new List<AttributeCollection>();

            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode)) // Array
            {
                foreach (var item in (JsonArray)consumedCapacityNode)
                {
                    var unit = item.As<ConsumedCapacity>();

                    // TODO
                }
            }

            TableItemCollection[] responses;

            if (json.TryGetValue("Responses", out var responsesNode))
            {
                var tableElements = (JsonObject)responsesNode;

                var collections = new TableItemCollection[tableElements.Keys.Count];

                int i = 0;

                foreach (var tableEl in tableElements)
                {
                    var table = new TableItemCollection(tableEl.Key);

                    foreach (var item in (JsonArray)tableEl.Value)
                    {
                        table.Add(AttributeCollection.FromJson((JsonObject)item));
                    }

                    collections[i] = table;

                    i++;
                }

                responses = collections;
            }
            else
            {
                responses = Array.Empty<TableItemCollection>();
            }

            /*
			if (json.TryGetValue("UnprocessedKeys", out var unprocessedKeysNode))
			{
                var unprocessedKeys = new List<TableKeys>();

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

            return new BatchGetItemResult(responses);
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
