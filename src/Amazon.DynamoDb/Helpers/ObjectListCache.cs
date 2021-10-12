// Based on .NET Source code

namespace System.Text
{
    internal static class ObjectListCache<T>
    {
        private static readonly List<List<T>> pool = new ();
        private static object lockObject = new ();

        internal struct Handle : IDisposable
        {
            public List<T> Value;

            public Handle(List<T> value)
            {
                Value = value;
            }

            public void Dispose()
            {
                Release(Value);
            }
        }

        public static Handle AcquireHandle()
        {
            lock (lockObject)
            {
                if (pool.Count == 0)
                {
                    return new Handle(new List<T>());
                }
                else
                {
                    var handle = new Handle(pool[^1]);
                    pool.RemoveAt(pool.Count - 1);
                    return handle;
                }
            }
        }

        public static void Release(List<T> list)
        {
            list.Clear();

            lock (lockObject)
            {
                pool.Add(list);
            }
        }
    }
}