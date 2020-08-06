using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class TransactGetItemsResult : IConsumedResources
    {
        public ConsumedCapacity? ConsumedCapacity { get; set; }
        public ItemResult[]? Responses { get; set; }
    }

    public class ItemResult
    {
        public AttributeCollection? Item { get; set; }
    }
}
