using Carbon.Data;

namespace Amazon.DynamoDb.Converters;

internal sealed class SingleConverter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember? member) => new DbValue((float)value);

    public object ToObject(DbValue item, IMember? member) => item.ToSingle();
}