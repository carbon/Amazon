#nullable disable

using System.Collections.Generic;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class BatchWriteItemResult // : IConsumedResources
    {
        // public ConsumedCapacity[] ConsumedCapacity { get; set; }

        public List<TableRequests> UnprocessedItems { get; set; }

        public static BatchWriteItemResult FromJsonElement(in JsonElement json)
        {
            var unprocessed = new List<TableRequests>();

            /*
            if (json.TryGetValue("ConsumedCapacity", out var consumedCapacityNode)) // Array
            {
                foreach (var item in (JsonArray)consumedCapacityNode)
                {
                    var unit = item.As<ConsumedCapacity>();

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