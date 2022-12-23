using Microsoft.Extensions.ObjectPool;

namespace Amazon.DynamoDb;

internal class ObjectPoolListPolicy<T> : IPooledObjectPolicy<List<T>>
{
    public List<T> Create()
    {
        return new List<T>(4);
    }

    public bool Return(List<T> obj)
    {
        obj.Clear();

        return obj.Capacity <= 32;
    }
}
