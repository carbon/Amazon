#nullable disable

using System.Collections.Generic;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class BatchWriteItemResult // : IConsumedResources
    {
        // public ConsumedCapacity[] ConsumedCapacity { get; init; }

        public List<TableRequests> UnprocessedItems { get; init; }

        public static BatchWriteItemResult FromJsonElement(in JsonElement json)
        {
            var unprocessed = new List<TableRequests>();

            /*
            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityEl)) // Array
            {
                foreach (var el in consumedCapacityNode.EnumerateArray())
                {
                    var unit = ConsumedCapacity.FromJsonElement(el);

                    // TODO
                }
            }
            */

            if (json.TryGetProperty("UnprocessedItems", out var unprocessedItemsEl))
            {
                foreach (JsonProperty batch in unprocessedItemsEl.EnumerateObject())
                {
                    unprocessed.Add(TableRequests.FromJsonElement(batch.Name, batch.Value));
                }
            }

            return new BatchWriteItemResult {
                UnprocessedItems = unprocessed
            };
        }
    }
}