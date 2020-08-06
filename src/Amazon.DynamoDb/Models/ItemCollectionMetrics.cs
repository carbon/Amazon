using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public sealed class ItemCollectionMetrics
    {
        public Dictionary<string, DbValue> ItemCollectionKey { get; set; }
        public float[] SizeEstimateRangeGB { get; set; }
    }
}
