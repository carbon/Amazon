using System.Collections;
using System.Collections.Generic;

namespace Amazon.DynamoDb
{
	public sealed class QueryResult<T> : IReadOnlyList<T>, IConsumedResources
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

        public AttributeCollection? LastEvaluatedKey { get; }

        public ConsumedCapacity? ConsumedCapacity { get; }

		#region IReadOnlyList

		public T this[int index] => items[index];
	
        public int Count => items.Length;

		#endregion		

		#region IEnumerable

		public IEnumerator<T> GetEnumerator() => ((IList<T>)items).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

		#endregion
	}
}
