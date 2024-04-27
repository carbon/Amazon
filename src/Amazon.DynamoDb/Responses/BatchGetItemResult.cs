using System.Text.Json;

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
        }

        // TODO:
        // UnprocessedKeys

        return new BatchGetItemResult {
            Responses = responses ?? [],
            ConsumedCapacity = consumedCapacity
        };
    }
}