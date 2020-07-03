using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class DeleteItemResult : IConsumedResources
    {
        public DeleteItemResult(AttributeCollection attributes, ConsumedCapacity? consumedCapacity)
        {
            Attributes = attributes;
            ConsumedCapacity = consumedCapacity;
        }
        
        public AttributeCollection Attributes { get; }

        public ConsumedCapacity? ConsumedCapacity { get; }

        public static DeleteItemResult FromJsonElement(JsonElement json)
        {
            AttributeCollection? attributes = null;
            ConsumedCapacity? consumedCapacity = null;

            if (json.TryGetProperty("Attributes", out var attributesEl))
            {
                attributes = AttributeCollection.FromJsonElement(attributesEl);
            }

            if (json.TryGetProperty("ConsumedCapacity", out var consumedCapacityEl))
            {
                consumedCapacity = ConsumedCapacity.FromJsonElement(consumedCapacityEl);
            }

            return new DeleteItemResult(attributes!, consumedCapacity);
        }
    }
}