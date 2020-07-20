#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class BatchGetItemResult // : IConsumedResources
    {
        public BatchGetItemResult() { }
        public BatchGetItemResult(IReadOnlyList<TableItemCollection> responses)
        {
            this.Responses = responses;
        }

        // public ConsumedCapacity[] ConsumedCapacity { get; set; }

        public IReadOnlyList<TableItemCollection> Responses { get; }

        public IReadOnlyList<TableKeys> UnprocessedKeys => Array.Empty<TableKeys>();

        public static BatchGetItemResult FromJsonElement(JsonElement json)
        {
            // TODO: ConsumedCapacity

            IReadOnlyList<TableItemCollection> responses;

            if (json.TryGetProperty("Responses", out JsonElement responsesNode))
            {
                var collections = new List<TableItemCollection>();

                foreach (var tableEl in responsesNode.EnumerateObject()) // table elements
                {
                    var table = new TableItemCollection(tableEl.Name);

                    foreach (JsonElement item in tableEl.Value.EnumerateArray())
                    {
                        table.Add(AttributeCollection.FromJsonElement(item));
                    }

                    collections.Add(table);
                }

                responses = collections;
            }
            else
            {
                responses = Array.Empty<TableItemCollection>();
            }

            // TODO: UnprocessedKeys

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
