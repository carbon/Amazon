using System.Collections.Generic;

namespace Amazon.DynamoDb
{
    internal static class KeyExtensions
    {
        public static IReadOnlyDictionary<string, DbValue> ToReadOnlyDictionary(this IEnumerable<KeyValuePair<string, object>> key)
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