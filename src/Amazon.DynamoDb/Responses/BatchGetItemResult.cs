﻿using System.Collections.ObjectModel;
using System.Text.Json;

namespace Amazon.DynamoDb;

public sealed class BatchGetItemResult
{
    public BatchGetItemResult(IReadOnlyList<TableItemCollection> responses, ConsumedCapacity[]? consumedCapacity = null)
    {
        Responses = responses;
        ConsumedCapacity = consumedCapacity;
    }

    public ConsumedCapacity[]? ConsumedCapacity { get; }

    public IReadOnlyList<TableItemCollection> Responses { get; }

    public IReadOnlyList<TableKeys>? UnprocessedKeys { get; init; }

    public static BatchGetItemResult FromJsonElement(in JsonElement json)
    {
        IReadOnlyList<TableItemCollection>? responses = null;
        ConsumedCapacity[]? consumedCapacity = null;

        foreach (var property in json.EnumerateObject())
        {
            if (property.NameEquals("ConsumedCapacity"u8))
            {
                consumedCapacity = JsonSerializer.Deserialize<ConsumedCapacity[]>(property.Value);
            }

            else if (property.NameEquals("Responses"u8))
            {
                var collections = new List<TableItemCollection>();

                foreach (var tableEl in property.Value.EnumerateObject()) // table elements
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
        }

        // TODO: UnprocessedKeys

        return new BatchGetItemResult(responses ?? [], consumedCapacity);
    }
}

public sealed class TableItemCollection(string name) : Collection<AttributeCollection>
{
    public string Name { get; } = name;
}
