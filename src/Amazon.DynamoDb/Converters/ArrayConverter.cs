using Carbon.Data;

namespace Amazon.DynamoDb.Converters;

internal sealed class ArrayConverter<T> : IDbValueConverter
{
    public DbValue FromObject(object value, IMember? member) => new DbValue((T[])value);

    public object ToObject(DbValue item, IMember? member) => item.ToArray<T>();
}