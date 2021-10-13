using Carbon.Data;

namespace Amazon.DynamoDb;

internal sealed class DecimalConverter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember member)
    {
        return new DbValue((decimal)value);
    }

    public object ToObject(DbValue item, IMember member) => item.ToDecimal();
}
