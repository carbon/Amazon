#nullable disable

namespace Amazon.DynamoDb;

public sealed class ItemCollectionMetrics
{
    public Dictionary<string, DbValue> ItemCollectionKey { get; init; }

    public float[] SizeEstimateRangeGB { get; init; }
}