using Carbon.Data;

namespace Amazon.DynamoDb.Converters;

internal sealed class BinaryConverter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember? member)
    {
        byte[] data = (byte[])value;

        return data.Length > 0
            ? new DbValue(data, DbValueType.B)
            : DbValue.Empty;
    }

    public object ToObject(DbValue item, IMember? member) => item.ToBinary();
}