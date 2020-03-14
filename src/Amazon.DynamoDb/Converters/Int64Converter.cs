using Carbon.Data;

namespace Amazon.DynamoDb
{
    internal sealed class Int64Converter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) => new DbValue((long)value);

        public object ToObject(DbValue item, IMember member) => item.ToInt64();
    }
}