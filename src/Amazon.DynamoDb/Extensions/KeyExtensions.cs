namespace Amazon.DynamoDb
{
    internal static class KeyExtensions
    {
        public static Dictionary<string, DbValue> ToDictionary(this IEnumerable<KeyValuePair<string, object>> key)
        {
            var collection = new Dictionary<string, DbValue>();

            foreach (var item in key)
            {
                collection.Add(item.Key, new DbValue(item.Value));
            }

            return collection;
        }
    }
}