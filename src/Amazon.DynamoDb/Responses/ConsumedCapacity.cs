#nullable disable

using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class ConsumedCapacity
    {
        public string TableName { get; init; }

        public float CapacityUnits { get; init; }

        internal static ConsumedCapacity FromJsonElement(in JsonElement el)
        {
            return new ConsumedCapacity
            {
                TableName = el.TryGetProperty(nameof(TableName), out var tableNameEl) ? tableNameEl.GetString() : null,
                CapacityUnits = el.GetProperty(nameof(CapacityUnits)).GetInt32()
            };
        }
    }
}