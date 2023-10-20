using Carbon.Data;

namespace Amazon.DynamoDb.Converters;

internal sealed class BooleanConverter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember? member)
    {
        return new DbValue((bool)value);
    }

    public object ToObject(DbValue item, IMember? member)
    {
        return item.ToBoolean();
    }
}