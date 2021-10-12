namespace Amazon
{
    internal static class CollectionExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int batchSize)
        {
            var batch = new List<T>(batchSize);

            foreach (T item in items)
            {
                batch.Add(item);

                if (batch.Count == batchSize)
                {
                    yield return batch;

                    batch = new List<T>(batchSize);
                }
            }

            if (batch.Count > 0) yield return batch;
        }
    }
}
