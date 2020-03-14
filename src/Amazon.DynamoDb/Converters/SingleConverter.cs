using System;

using Carbon.Data;

namespace Amazon.DynamoDb
{
    internal sealed class SingleConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) => new DbValue((single)value);

        public object ToObject(DbValue item, IMember member) => item.ToSingle();
    }
}