#nullable disable

namespace Amazon.DynamoDb
{
    public sealed class UpdateItemResult : IConsumedResources
    {
        public UpdateItemResult() { }

        public UpdateItemResult(AttributeCollection attributes, ConsumedCapacity consumedCapacity)
        {
            Attributes = attributes;
            ConsumedCapacity = consumedCapacity;
        }

        public AttributeCollection Attributes { get; set; }

        public ConsumedCapacity ConsumedCapacity { get; set; }
    }
}