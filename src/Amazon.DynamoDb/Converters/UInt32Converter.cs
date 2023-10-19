using Carbon.Data;

namespace Amazon.DynamoDb;

internal sealed class UInt32Converter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember? member) => new DbValue((uint)value);

    public object ToObject(DbValue item, IMember? member) => item.ToUInt32();
}
