#nullable disable

namespace Amazon.DynamoDb
{
    public sealed class ConsumedCapacity
    {
        public string TableName { get; set; }

        public float CapacityUnits { get; set; }
    }
}