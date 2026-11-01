using System.Text.Json;

using Amazon.DynamoDb.Extensions;
using Amazon.DynamoDb.Serialization;

namespace Amazon.DynamoDb;

public sealed class BatchGetItemResult
{
    public required IReadOnlyList<TableItemCollection> Responses { get; init; }

    public ConsumedCapacity[]? ConsumedCapacity { get; init; }

    public IReadOnlyList<TableKeys>? UnprocessedKeys { get; init; }

    public static BatchGetItemResult Deserialize(in JsonElement json)
    {
        IReadOnlyList<TableItemCollection>? responses = null;
        ConsumedCapacity[]? consumedCapacity = null;
        IReadOnlyList<TableKeys>? unprocessedKeys = null;

        foreach (var property in json.EnumerateObject())
        {
            if (property.NameEquals("ConsumedCapacity"u8))
            {
                consumedCapacity = property.Value.Deserialize(DynamoDbSerializationContext.Default.ConsumedCapacityArray);
            }

            else if (property.NameEquals("Responses"u8))
            {
                var collections = new List<TableItemCollection>();

                foreach (var tableEl in property.Value.EnumerateObject()) // table elements
                {
                    var table = new TableItemCollection(tableEl.Name);

                    foreach (JsonElement itemEl in tableEl.Value.EnumerateArray())
                    {
                        table.Add(AttributeCollection.Deserialize(itemEl));
                    }

                    collections.Add(table);
                }

                responses = collections;
            }

            else if (property.NameEquals("UnprocessedKeys"u8))
            {
                var tables = new List<TableKeys>();

                foreach (var tableEl in property.Value.EnumerateObject())
                {
                    tables.Add(ReadTableKeys(tableEl));
                }

                unprocessedKeys = tables;
            }
        }

        return new BatchGetItemResult {
            Responses = responses ?? [],
            ConsumedCapacity = consumedCapacity,
            UnprocessedKeys = unprocessedKeys
        };
    }

    private static TableKeys ReadTableKeys(in JsonProperty table)
    {
        var keysEl = table.Value.GetProperty("Keys"u8);

        var keys = new Dictionary<string, DbValue>[keysEl.GetArrayLength()];

        int i = 0;

        foreach (JsonElement keyEl in keysEl.EnumerateArray())
        {
            keys[i] = new Dictionary<string, DbValue>(AttributeCollection.Deserialize(keyEl));

            i++;
        }

        return new TableKeys(table.Name, keys) {
            AttributesToGet = table.Value.TryGetProperty("AttributesToGet"u8, out var attributesEl) ? attributesEl.GetStringArray() : null,
            ConsistentRead  = table.Value.TryGetProperty("ConsistentRead"u8, out var consistentReadEl) && consistentReadEl.GetBoolean()
        };
    }
}