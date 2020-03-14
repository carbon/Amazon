using Carbon.Data;

namespace Amazon.DynamoDb
{
    internal sealed class DoubleConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) =>
            new DbValue((double)value);

        public object ToObject(DbValue item, IMember member) =>
            item.ToDouble();
    }
}