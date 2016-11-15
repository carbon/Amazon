namespace Amazon.DynamoDb
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	public class QueryResult<T> : IList<T>, IReadOnlyList<T>, IConsumedResources
	{
		private readonly T[] items;

		public QueryResult() { }

		public QueryResult(QueryResult result)
		{
            ConsumedCapacity = result.ConsumedCapacity;
            LastEvaluatedKey = result.LastEvaluatedKey;

            this.items = new T[result.Items.Count];

            for (int i = 0; i < items.Length; i++)
			{
				items[i] = result.Items[i].As<T>();
			}
		}

        public AttributeCollection LastEvaluatedKey { get; }

        public ConsumedCapacity ConsumedCapacity { get; }

		#region IReadOnlyList

		public T this[int index]
		{
			get { return items[index]; }
			set { throw new NotImplementedException(); }
		}

        public int Count => items.Length;

		#endregion

		#region IList<T>

		public int IndexOf(T item)
		    => ((IList<T>)items).IndexOf(item);

		public void Insert(int index, T item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public void Add(T item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

        public bool Contains(T item)
            => ((IList<T>)items).Contains(item);

		public void CopyTo(T[] array, int arrayIndex)
		    => ((IList<T>)items).CopyTo(array, arrayIndex);
		

        public bool IsReadOnly => true;

		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable

		public IEnumerator<T> GetEnumerator()
		    => ((IList<T>)items).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		    => items.GetEnumerator();

		#endregion
	}
}
