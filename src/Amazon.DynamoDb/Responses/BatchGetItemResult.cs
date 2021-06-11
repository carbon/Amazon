#pragma warning disable CA1507 // Use nameof to express symbol names

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class BatchGetItemResult
    {
        public BatchGetItemResult(IReadOnlyList<TableItemCollection> responses)
        {
            Responses = responses;
        }

        public ConsumedCapacity[]? ConsumedCapacity { get; init; }

        public IReadOnlyList<TableItemCollection> Responses { get; }

        public IReadOnlyList<TableKeys>? UnprocessedKeys { get; init; }

        public static BatchGetItemResult FromJsonElement(in JsonElement json)
        {
            IReadOnlyList<TableItemCollection> responses;
            ConsumedCapacity[]? consumedCapacity = null;

            if (json.TryGetProperty("ConsumedCapacity", out var consumedCapacityEl))
            {
                consumedCapacity = new ConsumedCapacity[consumedCapacityEl.GetArrayLength()];

                var i = 0;

                foreach (var el in consumedCapacityEl.EnumerateArray())
                {
                    consumedCapacity[i] = DynamoDb.ConsumedCapacity.FromJsonElement(el);
                }
            }

            if (json.TryGetProperty("Responses", out JsonElement responsesEl))
            {
                var collections = new List<TableItemCollection>();

                foreach (var tableEl in responsesEl.EnumerateObject()) // table elements
                {
                    var table = new TableItemCollection(tableEl.Name);

                    foreach (JsonElement itemEl in tableEl.Value.EnumerateArray())
                    {
                        table.Add(AttributeCollection.FromJsonElement(itemEl));
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

            return new BatchGetItemResult(responses) {
                ConsumedCapacity = consumedCapacity 
            };
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