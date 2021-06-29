using System.Collections.Generic;

namespace Amazon
{
    // TODO: Remove w/ .NET 6.0

    internal static class CollectionExtensions
    {
        public static IEnumerable<List<T>> Chunk<T>(this IEnumerable<T> items, int size)
        {
            var batch = new List<T>(size);

            foreach (T item in items)
            {
                batch.Add(item);

                if (batch.Count == size)
                {
                    yield return batch;

                    batch = new List<T>(size);
                }
            }

            if (batch.Count > 0) yield return batch;
        }
    }
}
