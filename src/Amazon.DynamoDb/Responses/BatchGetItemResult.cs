using System.Collections.Generic;
using System.Collections.ObjectModel;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class BatchGetItemResult // : IConsumedResources
    {
        // public ConsumedCapacity[] ConsumedCapacity { get; set; }

        public List<TableItems> Responses { get; } = new List<TableItems>();

        public List<TableKeys> UnprocessedKeys { get; } = new List<TableKeys>();

        public static BatchGetItemResult FromJson(JsonObject json)
        {
            var result = new BatchGetItemResult();

            var items = new List<AttributeCollection>();

            if (json.ContainsKey("ConsumedCapacity")) // Array
            {
                foreach (var item in (JsonArray)json["ConsumedCapacity"])
                {
                    var unit = ConsumedCapacity.FromJson((JsonObject)item);

                    // TODO
                }
            }

            if (json.ContainsKey("Responses"))
            {
                foreach (var tableEl in (JsonObject)json["Responses"])
                {
                    var table = new TableItems(tableEl.Key);

                    foreach (var item in (JsonArray)tableEl.Value)
                    {
                        table.Add(AttributeCollection.FromJson((JsonObject)item));
                    }

                    result.Responses.Add(table);
                }
            }

            /*
			if (json.ContainsKey("UnprocessedKeys"))
			{
				foreach (var tableEl in (JsonObject)json["UnprocessedKeys"])
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

    public class TableItems : Collection<AttributeCollection>
    {
        public TableItems(string name)
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
