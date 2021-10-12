using System.Collections;

namespace Amazon.DynamoDb
{
    public sealed class QueryResult<T> : IReadOnlyList<T>, IConsumedResources
		where T: notnull
	{
		private readonly T[] _items;

		public QueryResult(QueryResult result)
		{
            ConsumedCapacity = result.ConsumedCapacity;
            LastEvaluatedKey = result.LastEvaluatedKey;
            
            _items = new T[result.Items.Length];

            for (int i = 0; i < _items.Length; i++)
			{ 
				_items[i] = result.Items[i].As<T>();
			}
		}

        public Dictionary<string, DbValue>? LastEvaluatedKey { get; }

        public ConsumedCapacity? ConsumedCapacity { get; }

		public T this[int index] => _items[index];
	
        public int Count => _items.Length;

		// IEnumerable<T> ---

		public IEnumerator<T> GetEnumerator() => ((IList<T>)_items).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
	}
}
