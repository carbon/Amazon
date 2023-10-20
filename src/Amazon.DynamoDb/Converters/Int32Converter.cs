using Carbon.Data;

namespace Amazon.DynamoDb.Converters;

internal sealed class Int32Converter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember? member) => new DbValue((int)value);

    public object ToObject(DbValue item, IMember? member) => item.ToInt();
}