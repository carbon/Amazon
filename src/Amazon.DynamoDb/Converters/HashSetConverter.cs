using System.Linq;

using Carbon.Data;

namespace Amazon.DynamoDb;

internal sealed class HashSetConverter<T> : IDbValueConverter
{
    public DbValue FromObject(object value, IMember member)
    {
        return new DbValue(((HashSet<T>)value).ToArray());
    }

    public object ToObject(DbValue item, IMember member) => item.ToSet<T>();
}
