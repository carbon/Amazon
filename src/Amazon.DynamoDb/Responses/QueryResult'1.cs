using System.Collections;
using System.Collections.Generic;

namespace Amazon.DynamoDb
{
	public sealed class QueryResult<T> : IReadOnlyList<T>, IConsumedResources
		where T: notnull
	{
		private readonly T[] items;

		public QueryResult(QueryResult result)
		{
            ConsumedCapacity = result.ConsumedCapacity;
            LastEvaluatedKey = result.LastEvaluatedKey;
            
            this.items = new T[result.Items.Length];

            for (int i = 0; i < items.Length; i++)
			{ 
				items[i] = result.Items[i].As<T>();
			}
		}

        public Dictionary<string, DbValue>? LastEvaluatedKey { get; }

        public ConsumedCapacity? ConsumedCapacity { get; }

		// IReadOnlyList<T> ---

		public T this[int index] => items[index];
	
        public int Count => items.Length;

		// IEnumerable<T> ---

		public IEnumerator<T> GetEnumerator() => ((IList<T>)items).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
	}
}
