#pragma warning disable IDE0090 // Use 'new(...)'

using Carbon.Data;

namespace Amazon.DynamoDb;

internal sealed class DbValueArrayConverter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember member) => new DbValue((DbValue[])value);

    public object ToObject(DbValue item, IMember member) => item.ToArray<DbValue>();
}
